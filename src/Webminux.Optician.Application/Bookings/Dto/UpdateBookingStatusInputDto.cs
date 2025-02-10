using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Bookings.Dto
{
    /// <summary>
    /// DTO to update status of booking
    /// </summary>
    public class UpdateBookingStatusInputDto
    {
        /// <summary>
        /// Id of booking
        /// </summary>
        public int BookingId { get; set; }
    }
}
