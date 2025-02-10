using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.Authorization.Users;
using static Webminux.Optician.OpticianConsts;

namespace Webminux.Optician.Bookings
{
    public class BookingEmployee : Entity
    {
        [ForeignKey(nameof(Booking))]
        public virtual int BookingId { get; set; }
        public virtual Booking Booking { get; set; }

        [ForeignKey(nameof(Employee))]
        public virtual long EmployeeId { get; set; }
        public virtual User Employee { get; set; }

        public BookingEmployeeStatus Status { get; set; }
        protected BookingEmployee()
        {
        }

        public static BookingEmployee Create(int bookingId, long employeeId, BookingEmployeeStatus status)
        {
            return new BookingEmployee
            {
                BookingId = bookingId,
                EmployeeId = employeeId,
                Status = status
            };
        }
    }
}
