using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Bookings.Dto
{
    /// <summary>
    /// Input DTO for booking pagination method
    /// </summary>
    public class BookingPagedResultRequestDto : PagedResultRequestDto
    {
        /// <summary>
        /// User Type of currently logedIn user
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// Is User is Admin
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}
