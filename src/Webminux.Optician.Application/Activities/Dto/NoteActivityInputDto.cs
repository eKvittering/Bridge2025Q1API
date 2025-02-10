using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Application.Activities.Dto
{
    /// <summary>
    /// Dto for creating note activity
    /// </summary>
    public class NoteActivityInputDto : CreateActivityDto
    {
        ///<summary>
        ///Note of customer
        ///</summary>
        public string Description { get; set; }
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Customer Table Id
        /// </summary>
        public int? CustomerTableId { get; set; }
        public int? TicketId { get; set; }
        public List<long> EmployeeIds { get; set; }
        public List<int> GroupIds { get; set; }

    }
}
