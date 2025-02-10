using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Webminux.Optician.MultiTenancy;

namespace Webminux.Optician.MenuItems
{
    public class MenuItem : FullAuditedEntity, IMustHaveTenant
    {
        [ForeignKey(nameof(Tenant))]
        public int TenantId { get; set; }

        public string Name { get; set; }

        public int OrderNo{ get; set; }

        public bool IsActive { get; set; }

        public string RouteLink { get; set; }

        protected MenuItem()
        {

        }
        public static MenuItem Create(int tenantId, string name, string routerLink, int orderNo = 0, bool isActive = true)
        {
            MenuItem item = new MenuItem();
            item.TenantId = tenantId;
            item.Name = name;
            item.IsActive= isActive;
            item.RouteLink= routerLink;
            item.OrderNo = orderNo;
            return item;

        }

    }
}
