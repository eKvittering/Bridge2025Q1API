using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Webminux.Optician.SyncEconomicData.Dto
{
    public class SyncHistoryDetailDto : FullAuditedEntityDto
    {
        public virtual string HistoryType { get; set; }
        public virtual string HistoryObjectId { get; set; }
        public virtual string HistoryObjectTitle { get; set; }
        public virtual int EconomicSyncHistoryId { get; set; }
    }
}
