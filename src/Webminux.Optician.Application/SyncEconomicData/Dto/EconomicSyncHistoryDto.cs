using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician
{
    public class EconomicSyncHistoryDto : FullAuditedEntityDto
    {
        public virtual bool IsInProcess { get; set; }
        public virtual bool IsFailed { get; set; }
        public virtual int CustomersCount { get; set; }
        public virtual int ProductsCount { get; set; }
        public virtual int ProductsGroupsCount { get; set; }
        public virtual int InvoicesCount { get; set; }
    }
}
