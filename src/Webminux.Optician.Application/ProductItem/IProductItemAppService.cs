using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Webminux.Optician.Activities.Dto;
using Webminux.Optician.ProductItem.Dtos;

/// <summary>
/// Interface for ProductItem AppService
/// </summary>
public interface IProductItemAppService :IApplicationService
{
    /// <summary>
    /// Create a new ProductItem
    /// </summary>
    Task CreateAsync(CreateProductItemDto input);
    
    /// <summary>
    /// Update a ProductItem
    /// </summary>
    //Task UpdateAsync(UpdateProductItemDto input);

    /// <summary>
    /// Get a ProductItem
    /// </summary>
    Task<ProductItemDto> GetAsync(EntityDto input);

    /// <summary>
    /// Delete a ProductItem
    /// </summary>
    Task DeleteAsync(EntityDto input);
    
    /// <summary>
    /// Get all ProductItems
    /// </summary>
    Task<ListResultDto<ProductItemDto>> GetAllAsync();
    
    /// <summary>
    /// Get Paged ProductItems
    /// </summary>
    Task<PagedResultDto<ProductItemDto> > GetPagedResultAsync(PagedProductItemResultRequestDto input);

    /// <summary>
    /// Get all ProductItems
    /// </summary>
    Task<ListResultDto<ProductItemDto>> GetProductItemsOfProduct(EntityDto input);
    /// <summary>
    /// Sale Product Item
    /// </summary>
    /// <returns></returns>
    Task UpdateProductItemInformation(int productItemId,int? invoiceId,int? invoiceLineId,int? activityId,string note);

    Task ReturnSaleProductItem(int productItemId);
    Task<ProductItemActivityDto> GetProductItemByActivity(EntityDto input);
    Task UpdateProductItem(ProductItemActivityDto input);
    Task<List<ItemDetailDto>> IsAlreadyExist(IsAlreadySerialNumbers input);
    Task<ListResultDto<ProductItemDto>> GetProductItemsWithProductNumber(EntityDto<string> input);
}