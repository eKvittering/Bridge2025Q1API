using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.MultiTenancy;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Webminux.Optician.MultiTenancy.Dto
{
    [AutoMapTo(typeof(Tenant))]
    public class CreateTenantDto
    {
        [Required]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        //[RegularExpression(AbpTenantBase.TenancyNameRegex)]
        public string TenancyName { get; set; }

        [Required]
        [StringLength(AbpTenantBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string AdminEmailAddress { get; set; }

        [StringLength(AbpTenantBase.MaxConnectionStringLength)]
        public string ConnectionString { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public virtual string CompanyName { get; set; }

        [Required]
        public virtual string PrimaryColor { get; set; }

        [Required]
        public virtual string SecondaryColor { get; set; }
        public virtual string EconomicAgreementGrantToken { get; set; }
        public virtual string EconomicAppSecretToken { get; set; }
        public virtual string BillyAccessToken { get; set; }
        public virtual string BillyAppSecretToken { get; set; }
        public virtual string BillyAgreementGrantToken { get; set; }
        [Required]
        public virtual string Base64Logo { get; set; }
        [Required]
        public virtual int SyncApiId { get; set; }

        public virtual string CompanyType { get; set; }
        public virtual string Address { get; set; }
        public virtual string PostCode { get; set; }
        public virtual string Country { get; set; }
        public virtual bool IsEquipmentTypeMedical { get; set; }
        public virtual string InvoiceCurrency { get; set; }
        public virtual string WebAddress { get; set; }
        public virtual string TelephoneNumber { get; set; }

        /// <summary>
        /// Custom Fields 
        /// </summary>
        public virtual ICollection<CustomFieldDto> CustomFields { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CreateTenantDto()
        {
            CustomFields = new HashSet<CustomFieldDto>();
        }

    }
}
