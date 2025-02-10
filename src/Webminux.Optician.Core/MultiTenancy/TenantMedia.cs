using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.MultiTenancy
{
    public class TenantMedia : FullAuditedEntity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string HomeVideo { get; set; }
        public string HomeImage1 { get; set; }
        public string HomeImage2 { get; set; }
        public string HomeImage3 { get; set; }
        public string HomeImage4 { get; set; }
        public string HomeImage5 { get; set; }
        public string HomeImage6 { get; set; }
        public string HomeImage7 { get; set; }

        protected TenantMedia() { }
        public static TenantMedia Create(int tenantId, string mediaType, string url)
        {
            return new TenantMedia
            {
                TenantId = tenantId,
               
            };
        }

    }
}
