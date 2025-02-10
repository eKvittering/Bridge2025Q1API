using Abp.Application.Services.Dto;
using Abp.MultiTenancy;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Webminux.Optician.MultiTenancy.Dto
{
    public class TenantDto : CreationAuditedEntityDto
    {
        [Required]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        [RegularExpression(AbpTenantBase.TenancyNameRegex)]
        public string TenancyName { get; set; }

        [Required]
        [StringLength(AbpTenantBase.MaxNameLength)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int CompanyId { get; set; }
        [Required]
        public virtual string CompanyName { get; set; }

        [Required]
        public virtual string PrimaryColor { get; set; }

        [Required]
        public virtual string SecondaryColor { get; set; }

        public virtual string EconomicAgreementGrantToken { get; set; }

        public virtual string EconomicAppSecretToken { get; set; }

        public virtual string Base64Logo { get; set; }
        public virtual string LogoPath { get; set; }
        public virtual string BillyAgreementGrantToken { get; set; }
        public virtual string BillyAppSecretToken { get; set; }
        public virtual string BillyAccessToken { get; set; }
        public virtual int SyncApiId { get; set; }

        public virtual string CompanyType { get; set; }
        public virtual string Address { get; set; }
        public virtual string PostCode { get; set; }
        public virtual string Country { get; set; }
        public virtual bool IsEquipmentTypeMedical { get; set; }
        public virtual string InvoiceCurrency { get; set; }
        public virtual string WebAddress { get; set; }
        public virtual string TelephoneNumber { get; set; }
        public virtual ICollection<CustomFieldDto> CustomFields { get; set; }

        public TenantDto()
        {
            CustomFields = new HashSet<CustomFieldDto>();
        }
    }
}
