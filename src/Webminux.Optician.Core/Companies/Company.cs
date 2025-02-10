using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Webminux.Optician.MultiTenancy;
using static Webminux.Optician.OpticianConsts;

namespace Webminux.Optician.Companies
{
    public class Company : FullAuditedEntity, IMustHaveTenant
    {
        [ForeignKey(nameof(Tenant))]
        public virtual int TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string LogoUrl { get; set; }

        [Required]
        public virtual string LogoPublicId { get; set; }

        [Required]
        public virtual string PrimaryColor { get; set; } = "";

        [Required]
        public virtual string SecondaryColor { get; set; } = "";

        public virtual Tenant Tenant { get; set; }

        public virtual string EconomicAgreementGrantToken { get; set; }
        public virtual string EconomicAppSecretToken { get; set; }
        public virtual string BillyAgreementGrantToken { get; set; }
        public virtual string BillyAppSecretToken { get; set; }
        public virtual string BillyAccessToken { get; set; }
        public virtual SyncApis SyncApiId { get; set; }

        public virtual string CompanyType { get; set; }
        public virtual string Address { get; set; }
        public virtual string PostCode { get; set; }
        public virtual string Country { get; set; }
        public virtual bool IsEquipmentTypeMedical { get; set; }
        public virtual string InvoiceCurrency { get; set; }
        public virtual string WebAddress { get; set; }
        public virtual string TelephoneNumber { get; set; }

        protected Company() { }

        public static Company Create(int tenantId, string name, string logoUrl, string logoPublicId, string primaryColor, string secondaryColor,
            string economicAgreementGrantToken, string economicAppSecretToken, string billyAccessToken, string billyAppSecretToken,
            string billyAgreementGrantToken, int syncApiId, string companyType, string address, string postCode, string country, bool isEquipmentTypeMedical,
            string invoiceCurrency, string webAddress, string telephoneNumber)
        {
            var company = new Company()
            {
                TenantId = tenantId,
                Name = name,
                LogoUrl = logoUrl,
                LogoPublicId = logoPublicId,
                PrimaryColor = primaryColor,
                SecondaryColor = secondaryColor,
                EconomicAgreementGrantToken = economicAgreementGrantToken,
                EconomicAppSecretToken = economicAppSecretToken,
                BillyAccessToken = billyAccessToken,
                BillyAppSecretToken = billyAppSecretToken,
                BillyAgreementGrantToken = billyAgreementGrantToken,
                SyncApiId = (SyncApis)(int)syncApiId,
                CompanyType = companyType,
                Address = address,
                PostCode = postCode,
                Country = country,
                InvoiceCurrency = invoiceCurrency,
                TelephoneNumber = telephoneNumber,
                WebAddress = webAddress,
                IsEquipmentTypeMedical = isEquipmentTypeMedical,
            };
            return company;
        }
    }
}
