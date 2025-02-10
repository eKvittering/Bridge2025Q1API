using Abp.MultiTenancy;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Companies;

namespace Webminux.Optician.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public virtual Company Company { get; set; }
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
