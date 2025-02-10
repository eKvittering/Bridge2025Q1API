using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician
{
    public class EconomicSyncHistory : FullAuditedEntity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }
        public virtual bool IsInProcess { get; set; }
        public virtual bool IsFailed { get; set; }
        public virtual string ErrorLog { get; set; }
        protected EconomicSyncHistory() { }

        public static EconomicSyncHistory Create(int tenantId, bool isInProcess,bool isFailed=false, string errorLog = "")
        {
            return new EconomicSyncHistory
            {
                TenantId = tenantId,
                IsInProcess = isInProcess,
                IsFailed = isFailed,
                ErrorLog = errorLog
            };
        }
    }
}
