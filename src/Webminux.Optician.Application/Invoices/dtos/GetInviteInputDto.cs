using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Invoices.dtos
{
    /// <summary>
    /// Input DTO for get invites
    /// </summary>
    public class GetInviteInputDto
    {
        /// <summary>
        /// Id of invited group
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// Id of invited activity
        /// </summary>
        public int ActivityId { get; set; }

        /// <summary>
        /// Id of invited customer
        /// </summary>
        public int? CustomerId { get; set; }
    }
}
