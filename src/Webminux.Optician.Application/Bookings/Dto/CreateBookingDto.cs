using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Bookings.Dto
{
    /// <summary>
    /// DTO to create booking
    /// </summary>
    public class CreateBookingDto
    {
        /// <summary>
        /// Booking Starting Date
        /// </summary>
        public virtual string FromDate { get; set; }
        
        /// <summary>
        /// Booking Ending date
        /// </summary>
        public virtual string ToDate { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Ids of customer selected employees
        /// </summary>
        public List<long> EmployeeIds { get; set; }
        /// <summary>
        /// User Id for customer
        /// </summary>
        public long? CustomerUserId { get; set; }

        public int? BookingActivityTypeId { get; set; }
    }
}
