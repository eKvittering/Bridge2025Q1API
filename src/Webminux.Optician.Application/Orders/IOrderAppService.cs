using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Webminux.Optician.CommonDtos;
using Webminux.Optician.Orders.Dto;

namespace Webminux.Optician.Orders
{
    /// <summary>
    /// Provides Create,Get and Update Order
    /// </summary>
    public interface IOrderAppService : IApplicationService
    {

        /// <summary>
        /// Create Order
        /// </summary>
        /// <param name="input">Contain Order Basic information and List of Order Lines</param>
        Task CreateAsync(CreateOrderDto input);
    
        /// <summary>
        /// Get Order by Id
        /// </summary>
        /// <param name="input">Object That Contains Order Id</param>
        /// <returns>Order Object</returns>
        Task<OrderDto> GetAsync(EntityDto input);

        /// <summary>
        /// Update Order
        /// </summary>
        /// <param name="input">Contain Order Basic information and List of Order Lines</param>
        Task UpdateAsync(UpdateOrderDto input);

        /// <summary>
        /// Get All Orders With Pagination
        /// </summary>
        /// <param name="input">Object That Contains Filters,Page Size and Page Number</param>
        /// <returns>Paged Result of Orders</returns>
        Task<PagedResultDto<OrderDto>> GetAllAsync(PagedOrderResultRequestDto input);

        /// <summary>
        /// Provide Next Order number
        /// </summary>
        /// <returns>Order Number</returns>
        Task<SingleValueDto<string>> GetNextOrderNumber();

        /// <summary>
        /// Confirms that order is received
        /// </summary>
        /// <param name="input">Order Id</param>
        /// <returns>No Result</returns>
        Task MarkOrderAsReceived(EntityDto input);
    }
}
