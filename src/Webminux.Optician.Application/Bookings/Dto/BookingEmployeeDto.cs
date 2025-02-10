using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Bookings.Dto
{
    /// <summary>
    /// DTO for Employees of booking
    /// </summary>
    public class BookingEmployeeDto : EntityDto
    {

        /// <summary>
        /// Id of employee
        /// </summary>
        public virtual long EmployeeId { get; set; }

        /// <summary>
        /// Name of employee
        /// </summary>
        public virtual string EmployeeName { get; set; }

        /// <summary>
        /// Status according to employee response to booking request
        /// </summary>
        public virtual string Status { get; set; }
    }
}
