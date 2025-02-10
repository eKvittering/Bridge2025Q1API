using Abp.Application.Services.Dto;

namespace Webminux.Optician.Tickets.Dtos
{
    /// <summary>
    /// Data Transfer Object to update Ticket status
    /// </summary>
    public class UpdateTicketFollowUpDto : EntityDto
    {
        /// <summary>
        /// Integer Product Item
        /// </summary>
        public virtual int? GroupId { get; set; }

        /// <summary>
        /// Id of Product Item.
        /// </summary>
        public virtual long? EmployeeId { get; set; } = 0;
    }
}
