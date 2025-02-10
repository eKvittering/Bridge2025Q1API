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
    /// DTO to get detail of Booking
    /// </summary>
    public class BookingDto : EntityDto
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
        /// Description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Status of booking
        /// </summary>
        public virtual string BookingStatus { get; set; }

        /// <summary>
        /// Id of Activity Created for booking
        /// </summary>
        public virtual int ActivityId { get; set; }

        /// <summary>
        /// Provides type of booking
        /// </summary>
        public string BookingActivityType { get; set; }

        /// <summary>
        /// Name of customer
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets customer Id
        /// </summary>
        public long? CustomerId { get; set; }
        /// <summary>
        /// List of employees with there statuses against this booking request
        /// </summary>
        public ICollection<BookingEmployeeDto> Employees { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BookingDto()
        {
            Employees = new HashSet<BookingEmployeeDto>();
        }
    }
}