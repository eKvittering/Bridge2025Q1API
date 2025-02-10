using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Orders.Dto
{
    /// <summary>
    /// Input DTO to get Orders paginated records
    /// </summary>
    public class PagedOrderResultRequestDto : PagedResultRequestDto
    {
        /// <summary>
        /// Wild Card search string
        /// </summary>
        public virtual string Keyword { get; set; }
    }
}
