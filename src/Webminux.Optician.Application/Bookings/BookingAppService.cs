using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Bookings.Dto;
using Webminux.Optician.Core.Helpers;
using Webminux.Optician.Helpers;
using static Webminux.Optician.OpticianConsts;

namespace Webminux.Optician.Bookings
{
    /// <summary>
    /// Provide methods to Create, Get, GetPagedResult, UpdateEmployeeStatus, UpdateBookingStatus, AddComments And GetComments.
    /// </summary>

    [AbpAuthorize()]
    public class BookingAppService : OpticianAppServiceBase, IBookingAppService
    {
        #region Private variables
        private IBookingManager _bookingManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Activity> _activityRepository;
        private readonly IRepository<BookingEmployee> _bookingEmployeeRepository;
        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public BookingAppService(
            IBookingManager bookingManager,
            IRepository<User, long> userRepository,
            IRepository<Activity> activityRepository,
            IRepository<BookingEmployee> bookingEmployeeRepository)
        {
            _bookingManager = bookingManager;
            _userRepository = userRepository;
            _activityRepository = activityRepository;
            _bookingEmployeeRepository = bookingEmployeeRepository;
        }
        #endregion

        #region CreateAsync
        /// <summary>
        /// Create Booking against provided employees.
        /// </summary>
        /// <param name="createBookingDto">Contains Booking Dates and  Employee Ids</param>
        /// 
        [AbpAllowAnonymous]
        public async Task CreateAsync(CreateBookingDto createBookingDto)
        {
            var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
            var employees = createBookingDto.EmployeeIds
                .Select(e => BookingEmployee.Create(0, e, OpticianConsts.BookingEmployeeStatus.Pending)).ToList();
            var booking = Booking.Create(tenantId, createBookingDto.FromDate.ConvertDateTimeStringToDateTime(),
                createBookingDto.ToDate.ConvertDateTimeStringToDateTime(), BookingStatus.Open, 0, employees, createBookingDto.Description);
            booking.BookingActivityTypeId = createBookingDto.BookingActivityTypeId.HasValue ? createBookingDto.BookingActivityTypeId.Value : null;

            if (createBookingDto.CustomerUserId.HasValue)
                booking.CreatorUserId = createBookingDto.CustomerUserId.Value;
            await _bookingManager.CreateAsync(booking, createBookingDto.CustomerUserId.HasValue ? createBookingDto.CustomerUserId.Value : AbpSession.UserId.Value);
        }
        #endregion

        #region GetAsync
        /// <summary>
        /// Get the requested Booking with detail of booking employees
        /// </summary>
        /// <param name="input">Contains Id of booking</param>
        /// <returns>Booking DTO contains all booking information</returns>
        public async Task<BookingDto> GetAsync(EntityDto input)
        {
            var query = _bookingManager.GetAll();
            query = query.Where(b => b.Id == input.Id);
            IQueryable<BookingDto> selectQuery = GetBookingDetailSelectQuery(query);
            var booking = await selectQuery.FirstOrDefaultAsync();
            if (booking.CustomerId.HasValue)
            {
                var user = _userRepository.Get(booking.CustomerId.Value);
                if (user != null)
                {
                    booking.CustomerName = user.FullName;
                }
            }
            return booking;
        }

        /// <summary>
        /// Get the requested Booking with detail of booking employees on base of activity
        /// </summary>
        /// <param name="input">Contains Id of Activity</param>
        /// <returns>Booking DTO contains all booking information</returns>
        public async Task<BookingDto> GetByActivityIdAsync(EntityDto input)
        {
            var query = _bookingManager.GetAll();
            query = query.Where(b => b.ActivityId == input.Id);
            IQueryable<BookingDto> selectQuery = GetBookingDetailSelectQuery(query);
            var booking = await selectQuery.FirstOrDefaultAsync();
            booking.CustomerName=GetCustomerName(booking.CustomerId);
            return booking;
        }

        private string GetCustomerName(long? customerId)
        {
            var customerName = string.Empty;
            if (customerId.HasValue)
            {
                var user = _userRepository.Get(customerId.Value);
                if (user != null)
                {
                    customerName = user.FullName;
                }
            }

            return customerName;
        }

        private static IQueryable<BookingDto> GetBookingDetailSelectQuery(IQueryable<Booking> query)
        {
            return query.Select(b => new BookingDto
            {
                Id = b.Id,
                FromDate = b.FromDate,
                ToDate = b.ToDate,
                ActivityId = b.ActivityId,
                Description = b.Description,
                BookingStatus = b.BookingStatus.GetEnumValueAsString(),
                BookingActivityType = b.BookingActivityType.Name,
                CustomerId = b.CreatorUserId,
                Employees = b.BookingEmployees.Select(e => new BookingEmployeeDto
                {
                    Id = e.Id,
                    EmployeeId = e.EmployeeId,
                    EmployeeName = e.Employee.FullName,
                    Status = e.Status.GetEnumValueAsString()
                }).ToList()
            });
        }
        #endregion

        #region GetPagedResultAsync
        /// <summary>
        /// Provide Paged list of bookings
        /// </summary>
        /// <param name="input">DTO contain skip count, max result count and filters</param>
        /// <returns>Paged Result of requested bookings</returns>
        public async Task<PagedResultDto<BookingListDto>> GetPagedResultAsync(BookingPagedResultRequestDto input)
        {
            var query = _bookingManager.GetAll();
            if (!string.IsNullOrWhiteSpace(input.UserType))
            {
                if (input.UserType == OpticianConsts.UserTypes.Customer)
                {
                    query = query.Where(booking => booking.CreatorUserId == AbpSession.UserId.Value);
                }
                else if (input.UserType == UserTypes.Employee && !input.IsAdmin)
                {
                    query = query.Where(booking => booking.BookingEmployees.Select(booking => booking.EmployeeId).Contains(AbpSession.UserId.Value));
                }
            }

            IQueryable<BookingListDto> selectQuery = GetPagedResultSelectQuery(query);

            var pagedResult = await selectQuery.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
            foreach (var booking in pagedResult.Items)
            {
                booking.CustomerName=GetCustomerName(booking.CreatorUserId);
            }
            return pagedResult;
        }

        private static IQueryable<BookingListDto> GetPagedResultSelectQuery(IQueryable<Booking> query)
        {
            return query.Select(booking => new BookingListDto
            {
                Id = booking.Id,
                BookingStatus = booking.BookingStatus.GetEnumValueAsString(),
                FromDate = booking.FromDate,
                ToDate = booking.ToDate,
                CreationTime = booking.CreationTime,
                CreatorUserId = booking.CreatorUserId,
            });
        }


        #endregion

        #region UpdateEmployeeStatus
        /// <summary>
        /// Update status of current employee to accepted and mark other employee as NotAllowed
        /// </summary>
        /// <param name="input">Contains employeeId, Booking Id and status</param>
        public async Task UpdateEmployeeStatusAsAccepted(UpdateBookingEmployeeStatusInputDto input)
        {
            var bookingEmployees = await _bookingEmployeeRepository.GetAllListAsync(e => e.EmployeeId == input.EmployeeId && e.BookingId == input.BookingId);
            foreach (var employee in bookingEmployees)
                UpdateCurrentEmployeeStatusAsAcceptedAndOthersAsNotAllowed(input, employee);
        }

        private static void UpdateCurrentEmployeeStatusAsAcceptedAndOthersAsNotAllowed(UpdateBookingEmployeeStatusInputDto input, BookingEmployee employee)
        {
            employee.Status = BookingEmployeeStatus.Accepted;
        }

        /// <summary>
        /// Update Employee Status as Rejected
        /// </summary>
        /// <param name="input">EmployeeId and BookingId</param>
        public async Task UpdateEmployeeStatusAsRejected(UpdateBookingEmployeeStatusInputDto input)
        {
            var bookingEmployee = await _bookingEmployeeRepository.FirstOrDefaultAsync(e => e.EmployeeId == input.EmployeeId && e.BookingId == input.BookingId);
            bookingEmployee.Status = BookingEmployeeStatus.Rejected;
        }
        #endregion

        #region UpdateBookingStatus
        /// <summary>
        /// Update status of booking to close
        /// </summary>
        /// <param name="input">Contains booking Id and status</param>
        /// <returns></returns>
        public async Task UpdateBookingStatusAsClose(UpdateBookingStatusInputDto input)
        {
            var booking = await _bookingManager.GetAsync(input.BookingId);
            if (booking == null)
                throw new UserFriendlyException("Invalid Booking");

            booking.BookingStatus = BookingStatus.Close;

            var activity = await _activityRepository.GetAsync(booking.ActivityId);
            activity.IsClosed = true;
        }
        #endregion

        #region IsAllBookingEmployeesResponded
        public async Task<bool> IsAllBookingEmployeeHasResponded(EntityDto input)
        {
            var pendingBookingEmployeesCount = await _bookingEmployeeRepository.CountAsync(e => e.Status == BookingEmployeeStatus.Pending && e.BookingId == input.Id);
            return pendingBookingEmployeesCount == 0;
        }
        #endregion

    }
}
