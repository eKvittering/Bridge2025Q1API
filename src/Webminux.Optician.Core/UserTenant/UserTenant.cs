using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using static Webminux.Optician.OpticianConsts;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.MultiTenancy;

namespace Webminux.Optician
{
    public class UserTenant: Entity
    {
        public virtual int Id { get; set; }


        [ForeignKey(nameof(User))]
        public virtual long? UserId { get; set; }
        public virtual User User { get; set; }


        [ForeignKey(nameof(Tenant))]
        public virtual int TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }

        public UserTenant() { }

        public static UserTenant Create(int tenantId, int userId)
        {
            return new UserTenant
            {
                TenantId = tenantId,
                UserId = userId,
            };
        }

    }
}
