using Abp.Application.Services.Dto;

namespace Webminux.Optician.Tickets.Dtos
{
    /// <summary>
    /// Data transfer object user as input for Ticket pagination method
    /// </summary>
    public class PagedTicketRequestResultDto : PagedResultRequestDto
    {
        /// <summary>
        /// User Id of customer who created Ticket
        /// </summary>
        public long? CustomerUserId { get; set; }

        /// <summary>
        /// Id of employee who is responsible for Tickets
        /// </summary>
        public long? EmployeeId { get; set; }
        /// <summary>
        /// Wild Card Keyword
        /// </summary>
        public string Keyword { get; set; } = string.Empty;
        /// <summary>
        /// Opened only
        /// </summary>
        public bool ShowOnlyOpened { get; set; }

    }
}
