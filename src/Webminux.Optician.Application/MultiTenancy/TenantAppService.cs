using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Webminux.Optician.Authorization;
using Webminux.Optician.Authorization.Roles;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Companies;
using Webminux.Optician.Core;
using Webminux.Optician.CustomFields;
using Webminux.Optician.Editions;
using Webminux.Optician.Helpers;
using Webminux.Optician.MultiTenancy.Dto;
using static Webminux.Optician.OpticianConsts;

namespace Webminux.Optician.MultiTenancy
{
   // [AbpAuthorize(PermissionNames.Pages_Tenants)]
    public class TenantAppService : AsyncCrudAppService<Tenant, TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>, ITenantAppService
    {
        private readonly TenantManager _tenantManager;
        private readonly EditionManager _editionManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<UserType> _userTypeRepository;
        private readonly IAbpZeroDbMigrator _abpZeroDbMigrator;
        private readonly CompanyManager _companyManager;
        private readonly IMediaHelperService _imageHelperService;
        private readonly IConfiguration _configuration;
        private readonly IRepository<CustomField> _customFieldRepository;
        private readonly IRepository<TenantMedia> _tenantMediaRepository;
        public TenantAppService(
            IRepository<Tenant, int> repository,
            TenantManager tenantManager,
            EditionManager editionManager,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<UserType> userTypeRepository,
            IAbpZeroDbMigrator abpZeroDbMigrator,
            CompanyManager companyManager,
            IMediaHelperService imageHelperService,
            IConfiguration configuration,
            IRepository<CustomField> customFieldRepository,
            IRepository<TenantMedia> tenantMediaRepository)
            : base(repository)
        {
            _tenantManager = tenantManager;
            _editionManager = editionManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _userTypeRepository = userTypeRepository;
            _abpZeroDbMigrator = abpZeroDbMigrator;
            _companyManager = companyManager;
            _imageHelperService = imageHelperService;
            _configuration = configuration;
            _customFieldRepository = customFieldRepository;
            _tenantMediaRepository = tenantMediaRepository;
        }

        public override async Task<TenantDto> CreateAsync(CreateTenantDto input)
        {
            CheckCreatePermission();

            // Create tenant
            var tenant = ObjectMapper.Map<Tenant>(input);
            tenant.ConnectionString = input.ConnectionString.IsNullOrEmpty()
                ? null
                : SimpleStringCipher.Instance.Encrypt(input.ConnectionString);

            var defaultEdition = await _editionManager.FindByNameAsync(EditionManager.DefaultEditionName);
            if (defaultEdition != null)
            {
                tenant.EditionId = defaultEdition.Id;
            }

            await _tenantManager.CreateAsync(tenant);
            await CurrentUnitOfWork.SaveChangesAsync(); // To get new tenant's id.

            // Create tenant database
            _abpZeroDbMigrator.CreateOrMigrateForTenant(tenant);

            // We are working entities of new tenant, so changing tenant filter
            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {
                // Create static roles for new tenant
                CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));

                await CurrentUnitOfWork.SaveChangesAsync(); // To get static role ids

                // Grant all permissions to admin role
                var adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
                await _roleManager.GrantAllPermissionsAsync(adminRole);
                var userType = await _userTypeRepository.GetAll().FirstOrDefaultAsync(userType => userType.Name == OpticianConsts.UserTypes.Employee);
                // Create admin user for the tenant
                var adminUser = User.CreateTenantAdminUser(tenant.Id, input.AdminEmailAddress, userType.Id);
                await _userManager.InitializeOptionsAsync(tenant.Id);
                CheckErrors(await _userManager.CreateAsync(adminUser, User.DefaultPassword));
                await CurrentUnitOfWork.SaveChangesAsync(); // To get admin user's id

                // Assign admin user to role!
                CheckErrors(await _userManager.AddToRoleAsync(adminUser, adminRole.Name));
                await CurrentUnitOfWork.SaveChangesAsync();
                await CreateCompanyAsync(input, tenant);
                await AddCustomFields(input.CustomFields, tenant.Id);

                await CurrentUnitOfWork.SaveChangesAsync();
            }

            return MapToEntityDto(tenant);
        }



        public override async Task<TenantDto> UpdateAsync(TenantDto input)
        {
            CheckUpdatePermission();

            var tenant = await _tenantManager.GetByIdAsync(input.Id);

            ObjectMapper.Map(input, tenant);

            var company = await _companyManager.GetAsync(input.CompanyId);

            MediaUploadDto logoUploadResult = null;
            if (string.IsNullOrWhiteSpace(input.Base64Logo) == false)
            {
                await DeleteOldLogoAsync(company);
                logoUploadResult = await UploadLogoToCloudAsyc(input.Base64Logo);
            }
            MapUpdateTenantDtoToCompany(input, company, logoUploadResult);

            await UpdateExistingAndAddNewOneAsync(input);
            await CurrentUnitOfWork.SaveChangesAsync();

            return input;
        }

        private async Task UpdateExistingAndAddNewOneAsync(TenantDto tenant)
        {
            var oldCustomFields = await _customFieldRepository.GetAllListAsync(field => field.TenantId == tenant.Id);
            List<int> updateFieldIds = await UpdateExistingFieldsAsync(tenant, oldCustomFields);
            await FindAndCreateNewCustomFieldsAsync(tenant);
            await DeleteRemovedFieldsAsync(oldCustomFields, updateFieldIds);
        }

        private async Task<List<int>> UpdateExistingFieldsAsync(TenantDto tenant, List<CustomField> oldCustomFields)
        {
            var updatedFields = tenant.CustomFields.Where(field => field.Id > 0).ToList();
            var updateFieldIds = updatedFields.Select(field => field.Id).ToList();
            foreach (var field in updatedFields)
            {
                var updatedField = oldCustomFields.FirstOrDefault(f => f.Id == field.Id);
                updatedField.Screen = (Screen)field.Screen;
                updatedField.Type = (CustomFieldType)field.Type;
                updatedField.Label = field.Label;

                await _customFieldRepository.UpdateAsync(updatedField);
            }

            return updateFieldIds;
        }

        private async Task FindAndCreateNewCustomFieldsAsync(TenantDto tenant)
        {
            var newFields = tenant.CustomFields.Where(field => field.Id == 0).ToList();
            await AddCustomFields(newFields, tenant.Id);
        }

        private async Task DeleteRemovedFieldsAsync(List<CustomField> oldCustomFields, List<int> updateFieldIds)
        {
            var deletedFields = oldCustomFields.Where(oldField => updateFieldIds.Contains(oldField.Id) == false).ToList();
            foreach (var field in deletedFields)
            {
                await _customFieldRepository.HardDeleteAsync(field);
            }
        }

        public async Task<PagedResultDto<TenantDto>> GetPagedTenantsAsync(PagedTenantResultRequestDto input)
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MustHaveTenant, AbpDataFilters.MayHaveTenant))
            {
                var query = _userManager.Users;
                var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;

                var userName = query.Where(x => x.Id == AbpSession.UserId).Select(s => s.UserName).FirstOrDefault();
                var tenantIds = query.Where(x => x.UserName == userName).Select(s => s.TenantId).ToList();
                var filteredQuery = CreateFilteredQuery(input);
                filteredQuery = filteredQuery.Where(f => tenantIds.Contains(f.Id));
                var selecQuery = GetSelectQuery(filteredQuery);
                return await selecQuery.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
            }

            
        }

        private async Task DeleteOldLogoAsync(Company company)
        {
            var deleteLogoResult = await _imageHelperService.DeleteImageAsync(company.LogoPublicId);
            if (deleteLogoResult.Error != null)
            {
                throw new Exception(deleteLogoResult.Error.Message);
            }
        }

        private static void MapUpdateTenantDtoToCompany(TenantDto input, Company company, MediaUploadDto imageUploadResult)
        {
            company.Name = input.CompanyName;

            if (imageUploadResult != null)
            {
                company.LogoUrl = imageUploadResult.Url;
                company.LogoPublicId = imageUploadResult.PublicId;

            }
            company.PrimaryColor = input.PrimaryColor;
            company.SecondaryColor = input.SecondaryColor;
            company.EconomicAppSecretToken = input.EconomicAppSecretToken;
            company.EconomicAgreementGrantToken = input.EconomicAgreementGrantToken;
            company.CompanyType = input.CompanyType;
            company.Address = input.Address;
            company.PostCode = input.PostCode;
            company.Country = input.Country;
            company.IsEquipmentTypeMedical = input.IsEquipmentTypeMedical;
            company.InvoiceCurrency = input.InvoiceCurrency;
            company.WebAddress = input.WebAddress;
            company.TelephoneNumber = input.TelephoneNumber;
        }

        protected override IQueryable<Tenant> CreateFilteredQuery(PagedTenantResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.TenancyName.Contains(input.Keyword) || x.Name.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        protected override async Task<Tenant> GetEntityByIdAsync(int id)
        {
            var tenant = await Repository.GetAllIncluding(x => x.Company).FirstOrDefaultAsync(x => x.Id == id);
            if (tenant == null)
            {
                throw new EntityNotFoundException(typeof(Tenant), id);
            }

            return tenant;
        }


        protected override TenantDto MapToEntityDto(Tenant entity)
        {
            return new TenantDto
            {
                Id = entity.Id,
                Name = entity.Name,
                TenancyName = entity.TenancyName,
                IsActive = entity.IsActive,
                CreationTime = entity.CreationTime,
                CreatorUserId = entity.CreatorUserId,
                CompanyId = entity.Company == null ? -1 : entity.Company.Id,
                CompanyName = entity.Company == null ? string.Empty : entity.Company.Name,
                PrimaryColor = entity.Company == null ? string.Empty : entity.Company.PrimaryColor,
                SecondaryColor = entity.Company == null ? string.Empty : entity.Company.SecondaryColor,
                LogoPath = entity.Company == null ? string.Empty : entity.Company.LogoUrl,
                EconomicAgreementGrantToken = entity.Company == null ? string.Empty : entity.Company.EconomicAgreementGrantToken,
                EconomicAppSecretToken = entity.Company == null ? string.Empty : entity.Company.EconomicAppSecretToken,
                BillyAgreementGrantToken = entity.Company == null ? string.Empty : entity.Company.BillyAgreementGrantToken,
                BillyAccessToken = entity.Company == null ? string.Empty : entity.Company.BillyAccessToken,
                BillyAppSecretToken = entity.Company == null ? string.Empty : entity.Company.BillyAppSecretToken,
                SyncApiId = entity.Company == null ? 0 : (int)entity.Company.SyncApiId,
                CompanyType = entity.Company == null ? string.Empty : entity.Company.CompanyType,
                WebAddress = entity.Company == null ? string.Empty : entity.Company.WebAddress,
                TelephoneNumber = entity.Company == null ? string.Empty : entity.Company.TelephoneNumber,
                InvoiceCurrency = entity.Company == null ? string.Empty : entity.Company.InvoiceCurrency,
                Address = entity.Company == null ? string.Empty : entity.Company.Address,
                Country = entity.Company == null ? string.Empty : entity.Company.Country,
                IsEquipmentTypeMedical = entity.Company == null ? false : entity.Company.IsEquipmentTypeMedical,
                PostCode = entity.Company == null ? string.Empty : entity.Company.PostCode,

            };
        }

        protected override void MapToEntity(TenantDto updateInput, Tenant entity)
        {
            // Manually mapped since TenantDto contains non-editable properties too.
            entity.Name = updateInput.Name;
            entity.TenancyName = updateInput.TenancyName;
            entity.IsActive = updateInput.IsActive;
        }

        public override async Task DeleteAsync(EntityDto<int> input)
        {
            CheckDeletePermission();

            var tenant = await _tenantManager.GetByIdAsync(input.Id);
            await _tenantManager.DeleteAsync(tenant);
        }

        private void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        private async Task CreateCompanyAsync(CreateTenantDto input, Tenant tenant)
        {
            MediaUploadDto result = await UploadLogoToCloudAsyc(input.Base64Logo);
            var company = Company.Create(tenant.Id, input.CompanyName, result.Url, result.PublicId, input.PrimaryColor, input.SecondaryColor
            , input.EconomicAgreementGrantToken, input.EconomicAppSecretToken, input.BillyAccessToken, input.BillyAppSecretToken, input.BillyAppSecretToken, input.SyncApiId,
            input.CompanyType, input.Address, input.PostCode, input.Country, input.IsEquipmentTypeMedical, input.InvoiceCurrency,
            input.WebAddress, input.TelephoneNumber);
            await _companyManager.CreateAsync(company);
        }

        private async Task<MediaUploadDto> UploadLogoToCloudAsyc(string base64Logo)
        {
            var result = await _imageHelperService.AddMediaAsync(base64Logo);
            return result;
        }

        protected IQueryable<TenantDto> GetSelectQuery(IQueryable<Tenant> query)
        {
            return query.Select(entity =>

                new TenantDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    TenancyName = entity.TenancyName,
                    IsActive = entity.IsActive,
                    CreationTime = entity.CreationTime,
                    CreatorUserId = entity.CreatorUserId,
                    CompanyId = entity.Company == null ? -1 : entity.Company.Id,
                    CompanyName = entity.Company == null ? string.Empty : entity.Company.Name,
                    PrimaryColor = entity.Company == null ? string.Empty : entity.Company.PrimaryColor,
                    SecondaryColor = entity.Company == null ? string.Empty : entity.Company.SecondaryColor,
                    LogoPath = entity.Company == null ? string.Empty : entity.Company.LogoUrl,
                    EconomicAgreementGrantToken = entity.Company == null ? string.Empty : entity.Company.EconomicAgreementGrantToken,
                    EconomicAppSecretToken = entity.Company == null ? string.Empty : entity.Company.EconomicAppSecretToken,
                    BillyAgreementGrantToken = entity.Company == null ? string.Empty : entity.Company.BillyAgreementGrantToken,
                    BillyAccessToken = entity.Company == null ? string.Empty : entity.Company.BillyAccessToken,
                    BillyAppSecretToken = entity.Company == null ? string.Empty : entity.Company.BillyAppSecretToken,
                    SyncApiId = entity.Company == null ? 0 : (int)entity.Company.SyncApiId,
                    CompanyType = entity.Company == null ? string.Empty : entity.Company.CompanyType,
                    WebAddress = entity.Company == null ? string.Empty : entity.Company.WebAddress,
                    TelephoneNumber = entity.Company == null ? string.Empty : entity.Company.TelephoneNumber,
                    InvoiceCurrency = entity.Company == null ? string.Empty : entity.Company.InvoiceCurrency,
                    Address = entity.Company == null ? string.Empty : entity.Company.Address,
                    Country = entity.Company == null ? string.Empty : entity.Company.Country,
                    IsEquipmentTypeMedical = entity.Company == null ? false : entity.Company.IsEquipmentTypeMedical,
                    PostCode = entity.Company == null ? string.Empty : entity.Company.PostCode,
                });
        }

        private async Task AddCustomFields(ICollection<CustomFieldDto> customFields, int tenantId)
        {
            foreach (var field in customFields)
            {
                var customField = CustomField.Create(tenantId, field.Label, (OpticianConsts.CustomFieldType)field.Type, (OpticianConsts.Screen)field.Screen);
                await _customFieldRepository.InsertAsync(customField);
            }
        }

        [AbpAllowAnonymous]
        public async Task<TenantMediaDto> GetTenantMediaInfoAsync(string tenancyName)
        {
            var media = await _tenantMediaRepository.FirstOrDefaultAsync(t => t.Tenant.Name == tenancyName);
            if (media == null)
            {
                return null;
                //throw new EntityNotFoundException(typeof(TenantMedia), tenantId);
            }

            return MapTenantMediaIntoDto(media);
        }

        private static TenantMediaDto MapTenantMediaIntoDto(TenantMedia tenantMedia)
        {

            var dto = new TenantMediaDto();
            dto.Id = tenantMedia.Id;
            dto.TenantId = tenantMedia.TenantId;
            dto.HomeVideo = tenantMedia.HomeVideo;
            dto.HomeImage1 = tenantMedia.HomeImage1;
            dto.HomeImage2 = tenantMedia.HomeImage2;
            dto.HomeImage3 = tenantMedia.HomeImage3;
            dto.HomeImage4 = tenantMedia.HomeImage4;
            dto.HomeImage5 = tenantMedia.HomeImage5;
            dto.HomeImage6 = tenantMedia.HomeImage6;
            dto.HomeImage7 = tenantMedia.HomeImage7;

            return dto;
        }
    }
}

