using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Bookings.Dto
{
    /// <summary>
    /// DTO user for listing of Bookings
    /// </summary>
    public class BookingListDto : EntityDto, ICreationAudited
    {
        /// <summary>
        /// Booking starting date
        /// </summary>
        public virtual DateTime FromDate { get; set; }

        /// <summary>
        /// Booking Ending date
        /// </summary>
        public virtual DateTime ToDate { get; set; }

        /// <summary>
        /// Status of booking
        /// </summary>
        public virtual string BookingStatus { get; set; }

        /// <summary>
        /// Id of user who created booking
        /// </summary>
        public long? CreatorUserId { get; set; }

        /// <summary>
        /// Time of booking creation
        /// </summary>
        public DateTime CreationTime { get; set; }

        public string CustomerName { get; set; }
    }
}
