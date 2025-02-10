using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using Webminux.Optician.ProductGroups.Dto;
using Webminux.Optician.Products.Dto;
using Webminux.Optician.Products.Dtos;

namespace Webminux.Optician.Products
{
    /// <summary>
    /// The class that represents a product.
    /// </summary>
    public interface IProductItemAppService : IApplicationService
    {
        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>The list of products.</returns>
        Task<ListResultDto<LookUpDto<int>>> GetAll();

        /// <summary>
        /// Gets a product by its id.
        /// </summary>
        /// <param name="id">The id of the product.</param>
        /// <returns>The product.</returns>
        Task<ProductGroupDto> GetById(int id);

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="input">The product to create.</param>
        /// <returns>The created product.</returns>
        Task Create(ProductGroupDto input);

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="input">The product to update.</param>
        /// <returns>The updated product.</returns>
        Task Update(ProductGroupDto input);

        /// <summary>
        /// Deletes a product.
        /// </summary>
        /// <param name="input">The id of the product.</param>
        /// <returns>The deleted product.</returns>
        Task Delete(EntityDto input);


        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ProductGroupDto>> GetPagedResultAsync(PagedProductGroupResultRequestDto input);
    }
}
