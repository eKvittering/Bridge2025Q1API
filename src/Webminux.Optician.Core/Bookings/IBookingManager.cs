using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Bookings
{
    public interface IBookingManager : IDomainService
    {
        public Task CreateAsync(Booking booking, long customerUserId);
        public Task UpdateAsync(Booking booking);
        public Task<Booking> GetAsync(int bookingId);
        IQueryable<Booking> GetAll();
    }
}
