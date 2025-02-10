using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.Activities;
using static Webminux.Optician.OpticianConsts;

namespace Webminux.Optician.Bookings
{
    public class Booking : FullAuditedEntity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }
        public virtual DateTime FromDate { get; set; }
        public virtual DateTime ToDate { get; set; }
        public virtual BookingStatus BookingStatus { get; set; }
        public virtual string Description { get; set; }

        [ForeignKey(nameof(Activity))]
        public virtual int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        [ForeignKey(nameof(BookingActivityType))]
        public virtual int? BookingActivityTypeId { get; set; }
        public virtual BookingActivityType BookingActivityType { get; set; }

        public virtual List<BookingEmployee> BookingEmployees { get; set; }

        protected Booking()
        {
        }

        public static Booking Create(int tenantId, DateTime fromDate, DateTime toDate, BookingStatus bookingStatus,
            int activityId, List<BookingEmployee> bookingEmployees, string description)
        {
            return new Booking
            {
                TenantId = tenantId,
                FromDate = fromDate,
                ToDate = toDate,
                BookingStatus = bookingStatus,
                ActivityId = activityId,
                BookingEmployees = bookingEmployees,
                Description = description
            };
        }
    }
}
