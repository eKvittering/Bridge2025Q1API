using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Bookings.Dto
{
    /// <summary>
    /// DTO to Update Employee Status he choses. 
    /// </summary>
    public class UpdateBookingEmployeeStatusInputDto : UpdateBookingStatusInputDto
    {
        /// <summary>
        /// Id of employee
        /// </summary>
        public long EmployeeId { get; set; }
    }
}
