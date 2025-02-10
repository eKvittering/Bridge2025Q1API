using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Webminux.Optician.MultiTenancy;

namespace Webminux.Optician.PageConfigs
{
    public class PageConfig : FullAuditedEntity, IMustHaveTenant
    {
        [ForeignKey(nameof(Tenant))]
        public int TenantId { get; set; }

        public string Name { get; set; }

        public string Config { get; set; }

        protected PageConfig()
        {

        }
        public static PageConfig Create(int tenantId, string name, string config)
        {
            PageConfig item = new PageConfig();
            item.TenantId = tenantId;
            item.Name = name;
            item.Config = config;
            return item;

        }
    }
}
