
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Webminux.Optician.Application.Suppliers.Dtos;

namespace Webminux.Optician.Application.Suppliers
{
    /// <summary>
    /// Provides CRUD Oprations for Supplier.
    /// </summary>
    public interface ISupplierAppService : IApplicationService
    {
        /// <summary>
        /// This method creates a new supplier.
        /// </summary>
        Task CreateAsync(CreateSupplierDto input);

        /// <summary>
        /// This method updates a customer.
        /// </summary>
        Task UpdateAsync(SupplierDto input);

        /// <summary>
        /// This method deletes a customer.
        /// </summary>
        Task DeleteAsync(EntityDto input);

        /// <summary>
        /// This method gets a customer by id.
        /// </summary>
        Task<SupplierDto> GetAsync(EntityDto input);

        /// <summary>
        /// This method gets all customers.
        /// </summary>
        Task<ListResultDto<SupplierListDto>> GetAllAsync();

        /// <summary>
        /// This method gets paged result of customer with PagedCustomerResultRequestDto input.
        /// </summary>
        Task<PagedResultDto<SupplierDto>> GetPagedResultAsync(PagedSupplierResultRequestDto input);

        /// <summary>
        /// This method gets next customer number.
        /// </summary>
        Task<GetSupplierNoResultDto> GetNextSupplierNoAsync();

        Task<EntityDto> GetSupplierIdFromUserId(EntityDto<long> input);

    }
}