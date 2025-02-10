using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Webminux.Optician.Application.Suppliers.Dtos;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Helpers;
using Webminux.Optician.Suppliers;
using Webminux.Optician.Users;

namespace Webminux.Optician.Application.Suppliers
{
    /// <summary>
    /// Provide CRUD Operation for s.
    /// </summary>
    public class SupplierAppService : OpticianAppServiceBase, ISupplierAppService
    {
        private readonly IUserAppService _userAppService;
        private readonly IRepository<Supplier> _supplierRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SupplierAppService"/> class.
        /// </summary>
        public SupplierAppService(IUserAppService userAppService, IRepository<Supplier> supplierRepository)
        {
            _userAppService = userAppService;
            _supplierRepository = supplierRepository;
        }

        /// <summary>
        /// This method creates a new customer.
        /// </summary>
        public async Task CreateAsync(CreateSupplierDto input)
        {
            try
            {
                int tenantId = 0;
                if (AbpSession.TenantId != null && AbpSession.TenantId.Value > 0)
                {
                    tenantId = AbpSession.TenantId.Value;
                }
                else
                {
                    tenantId = input.TenantId;
                }
                input.IsActive = true;
                var user = await _userAppService.CreateAsync(input);
                var supplier = Supplier.Create(tenantId, input.SuplierNo, input.Address, input.Postcode, input.TownCity, input.Telephone, input.Fax, input.Website, user.Id);
                await _supplierRepository.InsertAsync(supplier);
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// This method delete a customer.
        /// </summary>
        public async Task DeleteAsync(EntityDto input)
        {
            var supplier = await _supplierRepository.GetAsync(input.Id);
            if (supplier == null)
                throw new EntityNotFoundException(typeof(Supplier), input.Id);

            await _supplierRepository.DeleteAsync(supplier);
        }

        /// <summary>
        /// This method gets  all customer.
        /// </summary>
        public async Task<ListResultDto<SupplierListDto>> GetAllAsync()
        {
            var suppliers = await _supplierRepository.GetAll().Select(c => new SupplierListDto
            {
                Id = c.Id,
                CustomerNo = c.SupplierNo,
                Name = c.User.Name,
                EmailAddress = c.User.EmailAddress,
                SupplierUserId = c.User.Id,
            }).ToListAsync();

            return new ListResultDto<SupplierListDto>(suppliers);
        }

        #region GetAsync
        /// <summary>
        /// This method gets a customer by id.
        /// </summary>
        public async Task<SupplierDto> GetAsync(EntityDto input)
        {
            var supplier = await _supplierRepository.GetAsync(input.Id);
            if (supplier == null)
                throw new EntityNotFoundException(typeof(Supplier), input.Id);

            SupplierDto supplierToReturn = GetSupplierDtoFromSupplier(supplier);

            var getUserInputDto = new EntityDto<long> { Id = supplier.UserId };
            var user = await _userAppService.GetAsync(getUserInputDto);

            if (user == null)
                throw new EntityNotFoundException(typeof(User), supplier.UserId);

            FillUserInformation(supplierToReturn, user);
            return supplierToReturn;
        }




        /// <summary>
        /// This method gets next customer number.
        /// </summary>
        public async Task<GetSupplierNoResultDto> GetNextSupplierNoAsync()
        {
            var customerCount = await _supplierRepository.GetAll().CountAsync();
            var customerNo = customerCount + 1;
            return new GetSupplierNoResultDto { Number = customerNo };
        }

        private static void FillUserInformation(SupplierDto supplierToReturn, Optician.Users.Dto.UserDto user)
        {
            supplierToReturn.UserTypeId = user.UserTypeId;
            supplierToReturn.FullName = user.FullName;
            supplierToReturn.RoleNames = user.RoleNames;
            supplierToReturn.EmailAddress = user.EmailAddress;
            supplierToReturn.IsActive = user.IsActive;
            supplierToReturn.CreationTime = user.CreationTime;
            supplierToReturn.LastLoginTime = user.LastLoginTime;
            supplierToReturn.Name = user.Name;
            supplierToReturn.Surname = user.Surname;
            supplierToReturn.UserName = user.UserName;
            supplierToReturn.UserId = user.Id;
        }

        private static SupplierDto GetSupplierDtoFromSupplier(Supplier supplier)
        {
            return new SupplierDto
            {
                Id = supplier.Id,
                SupplierNo = supplier.SupplierNo,
                Address = supplier.Address,
                Postcode = supplier.Postcode,
                TownCity = supplier.TownCity,
                Telephone = supplier.Telephone,
                Fax = supplier.Fax,
                Website = supplier.Website,
            };
        }
        #endregion

        #region  GetPagedResultAsync
        /// <summary>
        /// This method gets paged result of customer with PagedCustomerResultRequestDto input.
        /// </summary>
        public async Task<PagedResultDto<SupplierDto>> GetPagedResultAsync(PagedSupplierResultRequestDto input)
        {
            var query = _supplierRepository.GetAll();

            query = ApplyFilter(input, query);
            IQueryable<SupplierDto> selectQuery = GetSelectQuery(query);
            var pagedResult = await selectQuery.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
            return pagedResult;
        }

        private static IQueryable<SupplierDto> GetSelectQuery(IQueryable<Supplier> query)
        {
            return query.Select(c => new SupplierDto
            {
                Id = c.Id,
                SupplierNo = c.SupplierNo,
                Address = c.Address,
                Postcode = c.Postcode,
                TownCity = c.TownCity,
                Telephone = c.Telephone,
                Fax = c.Fax,
                Website = c.Website,
                UserId = c.UserId,
                UserName = c.User.UserName,
                Name = c.User.Name,
                FullName = c.User.FullName,
                EmailAddress = c.User.EmailAddress,
                CreationTime = c.User.CreationTime,
                UserTypeId = c.User.UserTypeId
            });
        }

        private static IQueryable<Supplier> ApplyFilter(PagedSupplierResultRequestDto input, IQueryable<Supplier> query)
        {
            if (string.IsNullOrWhiteSpace(input.Keyword) == false)
            {
                query = query.Where(
                    a => a.User.UserName.Contains(input.Keyword)
                                || a.User.UserType.Name.Contains(input.Keyword)
                                || a.User.EmailAddress.Contains(input.Keyword)
                                || string.Concat(a.User.Name, " ", a.User.Surname).Contains(input.Keyword

                ));
            }

            return query;
        }

        #endregion

        #region  UpdateAsync
        /// <summary>
        /// This method updates a customer.
        /// </summary>
        public async Task UpdateAsync(SupplierDto input)
        {
            await GetAndUpdateUser(input);

            var supplier = await _supplierRepository.GetAsync(input.Id);
            if (supplier == null)
                throw new EntityNotFoundException(typeof(Supplier), input.Id);

            ObjectMapper.Map(input, supplier);
        }
        private async Task GetAndUpdateUser(SupplierDto input)
        {
            var user = await _userAppService.GetAsync(new EntityDto<long> { Id = input.UserId });
            if (user == null)
                throw new EntityNotFoundException(typeof(User), input.UserId);

            user.Name = input.Name;
            user.Surname = input.Surname;
            user.EmailAddress = input.EmailAddress;
            user.IsActive = input.IsActive;
            user.UserTypeId = input.UserTypeId;
            user.RoleNames = input.RoleNames;
            await _userAppService.UpdateAsync(user);
        }
        #endregion

        #region GetSupplierIdFromUserId
        public async Task<EntityDto> GetSupplierIdFromUserId(EntityDto<long> input)
        {
            var user = await _supplierRepository.GetAll().Where(s => s.UserId == input.Id).FirstOrDefaultAsync();

            return new EntityDto { Id = user.Id };
        }
        #endregion
    }
}