using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.Bookings.Dto;

namespace Webminux.Optician.Bookings
{
    /// <summary>
    /// Provides methods to create, get,Update Booking Status,Add Comments and get paged result
    /// </summary>
    public interface IBookingAppService : IApplicationService
    {
        /// <summary>
        /// Create Booking against provided employees.
        /// </summary>
        /// <param name="createBookingDto">Contains Booking Dates and  Employee Ids</param>
        Task CreateAsync(CreateBookingDto createBookingDto);

        /// <summary>
        /// Get the requested Booking with detail of booking employees
        /// </summary>
        /// <param name="input">Contains Id of booking</param>
        /// <returns>Booking DTO contains all booking information</returns>
        Task<BookingDto> GetAsync(EntityDto input);

        /// <summary>
        /// Provide Paged list of bookings
        /// </summary>
        /// <param name="input">DTO contain skip count, max result count and filters</param>
        /// <returns>Paged Result of requested bookings</returns>
        Task<PagedResultDto<BookingListDto>> GetPagedResultAsync(BookingPagedResultRequestDto input);

        /// <summary>
        /// Update the status of employee and mark other employees not allowed.
        /// </summary>
        /// <param name="input">Contains employee Id,BookingId and status</param>
        Task UpdateEmployeeStatusAsAccepted(UpdateBookingEmployeeStatusInputDto input);

        /// <summary>
        /// Update status of booking 
        /// </summary>
        /// <param name="input">Contains bookingId and status</param>
        Task UpdateBookingStatusAsClose(UpdateBookingStatusInputDto input);

        /// <summary>
        /// Get the requested Booking with detail of booking employees on base of activity
        /// </summary>
        /// <param name="input">Contains Id of Activity</param>
        /// <returns>Booking DTO contains all booking information</returns>

        Task<BookingDto> GetByActivityIdAsync(EntityDto input);

        /// <summary>
        /// Return true if all employees has respond to booking 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> IsAllBookingEmployeeHasResponded(EntityDto input);

        /// <summary>
        /// Update Employee Status as Rejected
        /// </summary>
        /// <param name="input">EmployeeId and BookingId</param>
        Task UpdateEmployeeStatusAsRejected(UpdateBookingEmployeeStatusInputDto input);
    }
}
