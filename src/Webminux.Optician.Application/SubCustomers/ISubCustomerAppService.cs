using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Webminux.Optician.Application.Customers.Dtos;
using Webminux.Optician.Customers.Dtos;
using Webminux.Optician.SubCustomers.Dtos;

namespace Webminux.Optician.Application.SubCustomers
{
    /// <summary>
    /// This is an application service class for <see cref="SubCustomers"/> entity.
    /// </summary>
    public interface ISubCustomerAppService : IApplicationService
    {
        /// <summary>
        /// This method creates a new sub customer.
        /// </summary>
        Task CreateAsync(SubCustomerDto input);

        /// <summary>
        /// This method updates a sub customer.
        /// </summary>
        Task UpdateAsync(SubCustomerDto input);

        /// <summary>
        /// This method deletes a sub customer.
        /// </summary>
        Task DeleteAsync(EntityDto input);

        /// <summary>
        /// This method gets a sub customer by id.
        /// </summary>
        Task<SubCustomerDto> GetAsync(EntityDto input);

        /// <summary>
        /// This method gets all sub customers.
        /// </summary>
        Task<ListResultDto<SubCustomerDto>> GetAllAsync();

        /// <summary>
        /// Get Sub Customers of specific customer 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ListResultDto<SubCustomerDto>> GetAllOfCustomerAsync(EntityDto input);

        /// <summary>
        /// This method gets paged result of sub customer with PagedCustomerResultRequestDto input.
        /// </summary>
        Task<PagedResultDto<SubCustomerDto>> GetPagedResultAsync(PagedSubCustomerResultRequestDto input);
    }
}