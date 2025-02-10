using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.PageConfigs.Dto
{
    public class PageConfigDto : EntityDto, ICreationAudited
    {
        public virtual string Name { get; set; }

        public int TenantId { get; set; }

       
        public string Config { get; set; }

        /// <summary>
        /// Gets or sets Creator User Id.
        /// </summary>
        public long? CreatorUserId { get; set; }

        /// <summary>
        /// Gets or sets Creation Time.
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
