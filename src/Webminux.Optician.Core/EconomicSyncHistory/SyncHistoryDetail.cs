using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician
{
    public class SyncHistoryDetail : FullAuditedEntity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }
        public virtual string HistoryType { get; set; }
        public virtual string HistoryObjectId { get; set; }
        public virtual string HistoryObjectTitle { get; set; }
        [ForeignKey(nameof(EconomicSyncHistory))]
        public virtual int EconomicSyncHistoryId { get; set; }
        public virtual EconomicSyncHistory EconomicSyncHistory { get; set; }
        protected SyncHistoryDetail() { }

        public static SyncHistoryDetail Create(int tenantId, string historyType, string historyObjectId, string historyObjectTitle, int economicSyncHistoryId)
        {
            return new SyncHistoryDetail
            {
                TenantId = tenantId,
                HistoryType = historyType,
                HistoryObjectId = historyObjectId,
                HistoryObjectTitle = historyObjectTitle,
                EconomicSyncHistoryId = economicSyncHistoryId
            };
        }
    }
}
