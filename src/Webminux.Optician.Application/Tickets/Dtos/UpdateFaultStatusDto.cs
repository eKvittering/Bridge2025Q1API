using Abp.Application.Services.Dto;

namespace Webminux.Optician.Tickets.Dtos
{
    /// <summary>
    /// Data Transfer Object to update Ticket status
    /// </summary>
    public class UpdateTicketStatusDto:EntityDto
    {
        /// <summary>
        /// Integer value of Ticket status
        /// </summary>
        public virtual int TicketStatus { get; set; }
    }
}
