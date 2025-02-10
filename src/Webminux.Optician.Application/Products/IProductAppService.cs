using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.Products.Dto;
using Webminux.Optician.Products.Dtos;

namespace Webminux.Optician.Products
{
    /// <summary>
    /// The class that represents a product.
    /// </summary>
    public interface IProductAppService : IApplicationService
    {
        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>The list of products.</returns>
        Task<ListResultDto<ProductDto>> GetAll();

        /// <summary>
        /// Gets a product by its id.
        /// </summary>
        /// <param name="id">The id of the product.</param>
        /// <returns>The product.</returns>
        Task<ProductDto> GetById(int id);

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="product">The product to create.</param>
        /// <returns>The created product.</returns>
        Task CreateAsync(ProductDto product);

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="product">The product to update.</param>
        /// <returns>The updated product.</returns>
        Task Update(ProductDto product);

        /// <summary>
        /// Deletes a product.
        /// </summary>
        /// <param name="product">The id of the product.</param>
        /// <returns>The deleted product.</returns>
        Task Delete(EntityDto product);


        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ProductDto>> GetPagedResultAsync(PagedProductResultRequestDto input);
    }
}
