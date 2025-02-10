using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician
{
    /// <summary>
    /// DTO to filter activities and manage paging
    /// </summary>
    public class PagedActivityRequestResultDto : PagedResultRequestDto
    {
        /// <summary>
        /// Wild Card Keyword
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// Customer Id to filter activities
        /// </summary>
        public int? CustomerId { get; set; }

        /// <summary>
        /// Where Activity is FollowUp or not
        /// </summary>
        public bool? IsFollowUp { get; set; }

        /// <summary>
        /// Where Activity is FollowUp or not
        /// </summary>
        public bool? IsClosed { get; set; }
        public int? FollowUpActivityTypeId { get; set; }

        public bool IsFromRememberReport { get; set; } = false;
        public bool? IsOnlyMe { get; set; }
    }
}
