using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Webminux.Optician.Application.Customers.Dtos;
using Webminux.Optician.Customers.Dtos;

namespace Webminux.Optician.Application.Customers
{
    /// <summary>
    /// This is an application service class for <see cref="Customer"/> entity.
    /// </summary>
    public interface ICustomerAppService : IApplicationService
    {
        /// <summary>
        /// This method creates a new customer.
        /// </summary>
        Task<long> CreateAsync(CreateCustomerDto input);

        /// <summary>
        /// This method updates a customer.
        /// </summary>
        Task UpdateAsync(CustomerDto input);

        /// <summary>
        /// This method deletes a customer.
        /// </summary>
        Task DeleteAsync(EntityDto input);

        /// <summary>
        /// This method gets a customer by id.
        /// </summary>
        Task<CustomerDto> GetAsync(EntityDto input);

        /// <summary>
        /// This method gets all customers.
        /// </summary>
        Task<ListResultDto<CustomerListDto>> GetAllAsync();

        /// <summary>
        /// This method gets paged result of customer with PagedCustomerResultRequestDto input.
        /// </summary>
        Task<PagedResultDto<CustomerDto>> GetPagedResultAsync(PagedCustomerResultRequestDto input);

        /// <summary>
        /// This method gets next customer number.
        /// </summary>
        Task<GetCustomerNoResultDto> GetNextCustomerNoAsync();

        /// <summary>
        /// Provide customer table id against user ID
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<EntityDto> GetCustomerIdFromUserIdAsync(EntityDto<long> input);
    }
}