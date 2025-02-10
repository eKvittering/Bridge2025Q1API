using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Faults.Dtos
{
    /// <summary>
    /// Data transfer object user as input for fault pagination method
    /// </summary>
    public class PagedFaultRequestResultDto : PagedResultRequestDto
    {
        /// <summary>
        /// User Id of customer who created fault
        /// </summary>
        public long? CustomerUserId { get; set; }

        /// <summary>
        /// Id of employee who is responsible for faults
        /// </summary>
        public long? ResponsibleEmployeeId { get; set; }

        /// <summary>
        /// Product Item Id 
        /// </summary>
        public int? ProductItemId { get; set; }

        /// <summary>
        /// Id of Supplier
        /// </summary>
        public int? SupplierId { get; set; }
    }
}
