using AutoMapper;

namespace Webminux.Optician.Tickets.Dtos
{
    /// <summary>
    /// Define mapping of Ticket DTO and Model
    /// </summary>
    public class TicketMapProfile:Profile
    {
        /// <summary>
        /// DeTicket Constructor
        /// </summary>
        public TicketMapProfile()
        {
            CreateMap<Ticket, TicketDto>();
            CreateMap<TicketDto, Ticket>();
        }
    }
}
