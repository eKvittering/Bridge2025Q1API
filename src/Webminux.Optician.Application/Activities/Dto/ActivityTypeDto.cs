using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Webminux.Optician
{
    /// <summary>
    /// Dto for ActivityType Model
    /// </summary>
    public class ActivityTypeDto : EntityDto,ILookupDto<int>
    {
        public virtual string Name { get; set; }
        public virtual int NextStepType { get; set; }
        public virtual int NextStepDage { get; set; }

    }
}
