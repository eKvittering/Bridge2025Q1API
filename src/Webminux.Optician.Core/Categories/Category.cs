using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Webminux.Optician.MultiTenancy;

namespace Webminux.Optician.Categories
{
    public class Category : FullAuditedEntity, IMustHaveTenant
    {
        [ForeignKey(nameof(Tenant))]
        public virtual int TenantId { get; set; }

        [Required]
        public string Name { get; set; }

        public int ParentCategoryId { get; set; }

        public bool IsDeactive { get; set; }

        protected Category()
        {

        }
        public static Category Create(int tenantId,string name,int parentCategoryId=0,bool isDeactive=false )
        {
            return new Category()
            {
                TenantId = tenantId,
                Name = name,
                ParentCategoryId = parentCategoryId,
                IsDeactive = isDeactive,

            };
        }

    }
}
