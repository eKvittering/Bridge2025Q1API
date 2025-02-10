using Abp.Application.Services.Dto;

namespace Webminux.Optician.Tickets.Dtos
{
    public class UpdateTicketSolutionNote : EntityDto
    {
        public string SolutionNote { get; set; }
    }
}
