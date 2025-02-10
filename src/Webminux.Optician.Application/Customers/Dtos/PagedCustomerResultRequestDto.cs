using Abp.Application.Services.Dto;

namespace Webminux.Optician.Application.Customers.Dtos
{
    /// <summary>
    /// This is a DTO class for <see cref="Customer"/> entity paged request.
    /// </summary>
    public class PagedCustomerResultRequestDto : PagedResultRequestDto
    {
        /// <summary>
        /// Get or Sets wild card search keyword.
        /// </summary>
        public string Keyword { get; set; }


        public bool showAllSubCustomers { get; set; } = true;

        /// <summary>
        /// Get or Sets customer type id.
        /// </summary>
        //public int? UserTypeId { get; set; }

        /// <summary>
        /// Get or Sets customer is active.
        /// </summary>
        // public bool? IsActive { get; set; }

        public long? CustomerUserId { get; set; } = 0;
        public string CustomerNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string TelePhone { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public int? CustomerTypeId { get; set; }

        public long? ResponsibleEmployeeId { get; set; }

    }
}