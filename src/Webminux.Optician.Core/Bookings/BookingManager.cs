using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.Activities;
using static Webminux.Optician.OpticianConsts;

namespace Webminux.Optician.Bookings
{
    public class BookingManager : IBookingManager
    {
        private readonly IActivityManager _activityManager;
        private readonly IRepository<Booking> _bookingRepository;
        private readonly IRepository<BookingEmployee> _bookingEmployeeRepository;
        private readonly IRepository<ActivityType> _activityTypeRepository;
        private readonly IRepository<ActivityArt> _activityArtRepository;
        private readonly ICustomerManager _customerManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAbpSession _session;
        #region Private Variables

        #endregion

        #region Constructor
        public BookingManager(
            IActivityManager activityManager,
            IRepository<Booking> bookingRepository,
            IRepository<BookingEmployee> bookingEmployeeRepository,
            IRepository<ActivityType> activityTypeRepository,
            IRepository<ActivityArt> activityArtRepository,
            ICustomerManager customerManager,
            IUnitOfWorkManager unitOfWorkManager,
            IAbpSession session
            )
        {
            _session = session;
            _activityManager = activityManager;
            _bookingRepository = bookingRepository;
            _bookingEmployeeRepository = bookingEmployeeRepository;
            _activityTypeRepository = activityTypeRepository;
            _activityArtRepository = activityArtRepository;
            _customerManager = customerManager;
            _unitOfWorkManager = unitOfWorkManager;
        }
        #endregion

        #region CreateAsync
        public async Task CreateAsync(Booking booking, long customerUserId)
        {
            var tenantId = _session.TenantId ?? OpticianConsts.DefaultTenantId;
            ActivityType activityType;
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                activityType = await _activityTypeRepository.
                      FirstOrDefaultAsync(activityType => activityType.Name == BookingConstants.ActivityType);
               
            }

            var activityArt = await _activityArtRepository
                .FirstOrDefaultAsync(activityArt => activityArt.Name == OpticianConsts.ActivityArtForPhoneCallActivity);

            ActivityType followUpType;
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                followUpType = await _activityTypeRepository.
               FirstOrDefaultAsync(activityType => activityType.Name == OpticianConsts.FollowUpActivityTypeForPhoneCallActivity);
            }
            var customer = await _customerManager.Customers.FirstOrDefaultAsync(customer => customer.UserId == customerUserId);
            var responsibleEmployee = customer.ResponsibleEmployeeId ?? customerUserId;

            var currentDate = DateTime.UtcNow;
            var activity = Activity.Create(tenantId,null, activityType.Name,null
                , currentDate, currentDate, activityType.Id, followUpType.Id,
                activityArt.Id, responsibleEmployee, customer.UserId, null);

            activity = await _activityManager.CreateAsync(activity);
            _unitOfWorkManager.Current.SaveChanges();

            booking.ActivityId = activity.Id;

            _bookingRepository.Insert(booking);
            await _unitOfWorkManager.Current.SaveChangesAsync();
        }
        #endregion

        #region GetAll
        public IQueryable<Booking> GetAll()
        {
            return _bookingRepository.GetAll();
        }
        #endregion

        #region GatAsync
        public async Task<Booking> GetAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetAsync(bookingId);
            booking.BookingEmployees = booking.BookingEmployees;
            return booking;
        }
        #endregion

        #region UpdateAsync
        public Task UpdateAsync(Booking booking)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
