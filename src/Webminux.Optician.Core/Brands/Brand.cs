using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Webminux.Optician.MultiTenancy;

namespace Webminux.Optician.Brands
{
    public class Brand : FullAuditedEntity, IMustHaveTenant
    {
        [ForeignKey(nameof(Tenant))]
        public virtual int TenantId { get; set; }

        [Required]
        public string Name { get; set; }

        public int ParentBrandId { get; set; }

        public bool IsDeactive { get; set; }

        protected Brand()
        {

        }
        public static Brand Create(int tenantId,string name , int parenBrandId=0,bool isDetactive = false)
        {
            Brand brand = new Brand();
            brand.TenantId = tenantId;
            brand.Name = name;
            brand.IsDeactive = isDetactive;
            return brand;

        }

    }
}
