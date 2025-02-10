
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webminux.Optician.Application;
using Webminux.Optician.Application.Customers.Dtos;
using Webminux.Optician.Application.Users.Dto;
using Webminux.Optician.Authorization;
using Webminux.Optician.Authorization.Roles;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Helpers;
using Webminux.Optician.Roles.Dto;
using Webminux.Optician.Users.Dto;

namespace Webminux.Optician.Users
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;
        private readonly IMediaHelperService _imageHelperService;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Handle User management .
        /// </summary>
        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager,
            IMediaHelperService imageHelperService,
             IMemoryCache cache
            )
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _imageHelperService = imageHelperService;
            _cache = cache;
        }

        /// <summary>
        /// Provide List of customers.
        /// </summary>
        [AbpAllowAnonymous]
        public async Task<ListResultDto<UserDto>> GetFilteredUsersAsync(GetAllInputDto input)
        {
            try
            {
                var users = await Repository.GetAllIncluding()
                .WhereIf(!input.UserType.IsNullOrWhiteSpace(), x => x.UserType.Name == input.UserType)
                .ToListAsync();
                return new ListResultDto<UserDto>(ObjectMapper.Map<List<UserDto>>(users));
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// 
        [AbpAllowAnonymous]
        public override async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            try
            {
                CheckCreatePermission();

                var user = ObjectMapper.Map<User>(input);

                user.TenantId = AbpSession.TenantId;
                user.IsEmailConfirmed = true;

                MediaUploadDto uploadResult = new MediaUploadDto();
                if (!string.IsNullOrWhiteSpace(input.Base64Picture))
                    uploadResult = await _imageHelperService.AddMediaAsync(input.Base64Picture);

                user.PicturePublicId = uploadResult.PublicId;
                user.PictureUrl = uploadResult.Url;

                await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

                CheckErrors(await _userManager.CreateAsync(user, input.Password));

                if (input.RoleNames != null)
                {
                    CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
                }

                CurrentUnitOfWork.SaveChanges();

                return MapToEntityDto(user);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Get All Users with pagination.
        /// </summary>    
        public async Task<PagedResultDto<UserListDto>> GetPagedResultAsync(PagedUserResultRequestDto input)
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MustHaveTenant, AbpDataFilters.MayHaveTenant))
            {
                var query = _userManager.Users;
                var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
                if (input.UserTypeId.HasValue)
                    query = query.Where(a => a.UserTypeId == input.UserTypeId);
                if (!string.IsNullOrWhiteSpace(input.Keyword))
                {
                    query = query.Where(a => a.UserName.Contains(input.Keyword)
                                    || a.UserType.Name.Contains(input.Keyword)
                                    || a.EmailAddress.Contains(input.Keyword)
                                    || string.Concat(a.Name, " ", a.Surname).Contains(input.Keyword));
                    // query=query.Where(c=>c.UserType.Name==OpticianConsts.UserTypes.Employee);
                }

                if(tenantId > 0)
                {
                    query = query.Where(a => a.TenantId == tenantId);
                }
                var selectQuery = GetUserListSelectQuery(query);
                var result = await selectQuery.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
                return result;
            }
        }




        /// <summary>
        /// Update existing user.
        /// </summary>    
        public override async Task<UserDto> UpdateAsync(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);

            MapToEntity(input, user);

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            return await GetAsync(input);
        }

        /// <summary>
        /// Delete existing user.
        /// </summary>
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        /// <summary>
        /// Activate existing user.
        /// </summary>
        [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task Activate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = true;
            });
        }

        /// <summary>
        /// Deactivate existing user.
        /// </summary>
        [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task DeActivateAync(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = false;
            });
        }

        /// <summary>
        /// Get all roles.
        /// </summary>
        public async Task<ListResultDto<RoleDto>> GetRolesAsync()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        /// <summary>
        /// Change user language.
        /// </summary>
        public async Task ChangeLanguageAsync(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        /// <summary>
        /// Map CreateUserDto to user model.
        /// </summary>
        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        /// <summary>
        /// Map user dto to user model.
        /// </summary>
        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        /// <summary>
        /// Map user model to user DTO.
        /// </summary>
        protected override UserDto MapToEntityDto(User user)
        {
            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();

            return userDto;
        }

        /// <summary>
        /// Provide filtered query for users.
        /// </summary>
        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword) || x.UserType.Name.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive)
                .WhereIf(input.UserTypeId.HasValue, x => x.UserTypeId == input.UserTypeId);
        }

        /// <summary>
        /// Get user with roles by id.
        /// </summary>
        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        /// <summary>
        /// Apply sorting to user.
        /// </summary>
        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        /// <summary>
        /// Validate Identity Result.
        /// </summary>
        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        /// <summary>
        /// Changes user password.
        /// </summary>
        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            var user = await _userManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            if (await _userManager.CheckPasswordAsync(user, input.CurrentPassword))
            {
                CheckErrors(await _userManager.ChangePasswordAsync(user, input.NewPassword));
            }
            else
            {
                CheckErrors(IdentityResult.Failed(new IdentityError
                {
                    Description = "Incorrect password."
                }));
            }

            return true;
        }
        /// <summary>
        /// Reset user password.
        /// </summary>
        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attempting to reset password.");
            }

            var currentUser = await _userManager.GetUserByIdAsync(_abpSession.GetUserId());
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            }

            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }

            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.Admin))
            {
                throw new UserFriendlyException("Only administrators may reset passwords.");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            return true;
        }

        private static IQueryable<UserListDto> GetUserListSelectQuery(IQueryable<User> query)
        {
            return query.Select(
                            u => new UserListDto
                            {
                                Id = u.Id,
                                UserName = u.UserName,
                                EmailAddress = u.EmailAddress,
                                FullName = u.FullName,
                                UserTypeName = u.UserType.Name,
                                CreationTime = u.CreationTime,
                                IsActive = u.IsActive,
                            }
                            );
        }

        public async Task<ListResultDto<UserDto>> GetAllUsers()
        {
            const string cacheKey = "AllUsers"; // Define a unique cache key

            // Try to get the cached data
            if (!_cache.TryGetValue(cacheKey, out List<UserDto> cachedUsers))
            {
                try
                {
                    // If cache is empty, fetch data from the repository
                    var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;

                    var users = await Repository.GetAllIncluding()
                        .Where(a => a.TenantId == tenantId)
                        .ToListAsync();

                    // Map the users to UserDto
                    cachedUsers = ObjectMapper.Map<List<UserDto>>(users);

                    // Set cache options
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Adjust the duration as needed

                    // Save data in cache
                    _cache.Set(cacheKey, cachedUsers, cacheEntryOptions);
                }
                catch (Exception ex)
                {
                    // Handle exceptions as necessary
                    throw; // Re-throw or handle as per your requirement
                }
            }

            // Return the cached users
            return new ListResultDto<UserDto>(cachedUsers);
        }

        //public async Task<ListResultDto<UserDto>> GetAllUsers()
        //{
        //    try
        //    {
        //        var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;

        //        var users =  await Repository.GetAllIncluding()
        //        .Where(a =>a.TenantId == tenantId)
        //        .ToListAsync();
        //        return new ListResultDto<UserDto>(ObjectMapper.Map<List<UserDto>>(users));
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        /// <summary>
        /// Get by User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserDto> GetByUserId(long userId)
        {
            try
            {
                var user = await Repository.FirstOrDefaultAsync(userId);

                var data = ObjectMapper.Map<UserDto>(user);

                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

