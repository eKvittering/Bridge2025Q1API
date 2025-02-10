using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.Faults.Dtos;

namespace Webminux.Optician.Faults
{
    /// <summary>
    /// Provide Methods to Get, Create and Update Status methods.
    /// </summary>
    public interface IFaultAppService : IApplicationService
    {
        /// <summary>
        /// Created fault against invoice line
        /// </summary>
        /// <param name="input">CreateFaultDto with description and InvoiceLineId properties</param>
        /// <returns></returns>
        Task CreateAsync(CreateFaultDto input);

        /// <summary>
        /// Find fault against invoice line
        /// </summary>
        /// <param name="input">Id of invoice line</param>
        /// <returns></returns>
        Task<ListResultDto<FaultDto>> FindInvoiceLineFaults(EntityDto input);

        /// <summary>
        /// Find paginated list of faults
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<FaultDto>> GetPagedResultAsync(PagedFaultRequestResultDto input);

        /// <summary>
        /// Update status of provided fault
        /// </summary>
        /// <param name="input">Contains fault status and fault Id</param>
        Task UpdateFaultStatus(UpdateFaultStatusDto input);

        /// <summary>
        /// Created fault From ERP system we are considering that Admin user is creating fault
        /// </summary>
        /// <param name="input">CreateFaultDto with description and InvoiceLineId properties</param>
        /// <returns></returns>
        Task CreateFromERPAsync(CreateERPFaultDto input);
        Task SaveFaultSolution(UpdateSolutionNote note);
    }
}
