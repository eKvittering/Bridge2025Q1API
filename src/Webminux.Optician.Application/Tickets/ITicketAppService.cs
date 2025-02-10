using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using Webminux.Optician.Tickets.Dtos;

namespace Webminux.Optician.Tickets
{
    /// <summary>
    /// Provide Methods to Get, Create and Update Status methods.
    /// </summary>
    public interface ITicketAppService : IApplicationService
    {
        /// <summary>
        /// Created Ticket against invoice line
        /// </summary>
        /// <param name="input">CreateTicketDto with description and InvoiceLineId properties</param>
        /// <returns></returns>
        Task CreateAsync(CreateTicketDto input);


        /// <summary>
        /// Find paginated list of Tickets
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<TicketDto>> GetPagedResultAsync(PagedTicketRequestResultDto input);

        /// <summary>
        /// Update status of provided Ticket
        /// </summary>
        /// <param name="input">Contains Ticket status and Ticket Id</param>
        Task UpdateTicketStatus(UpdateTicketStatusDto input);

        /// <summary>
        /// Created Ticket From ERP system we are considering that Admin user is creating Ticket
        /// </summary>
        /// <param name="input">CreateTicketDto with description and InvoiceLineId properties</param>
        /// <returns></returns>
        Task<int> CreateFromERPAsync(CreateERPTicketDto input);
        Task SaveTicketSolution(UpdateTicketSolutionNote note);
        Task UpdateTicketFollowType(UpdateTicketFollowUpDto input);
    }
}
