using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Webminux.Optician.Customers
{
    [Index(nameof(TenantId))]
    [Index(nameof(Type))]
    public class CustomerType : Entity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }
        public virtual string Type { get; set; }
        public virtual int? ObjectType { get; set; }

        protected CustomerType() { }

        public static CustomerType Create(int tenantId, string type, int? objecttype)
        {
            return new CustomerType()
            {
                TenantId = tenantId,
                Type = type,
                ObjectType = objecttype
            };
        }
    }
}
