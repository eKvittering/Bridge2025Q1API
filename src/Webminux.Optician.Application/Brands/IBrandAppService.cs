using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;


/// <summary>
/// Interface for Brand AppService
/// </summary>
public interface IBrandService :IApplicationService
{
    /// <summary>
    /// Create a new Brand
    /// </summary>
    Task CreateAsync(CreateBrandDto input);
    
    /// <summary>
    /// Update a Brand
    /// </summary>
    Task UpdateAsync(UpdateBrandDto input);

    /// <summary>
    /// Get a Brand
    /// </summary>
    Task<BrandDto> GetAsync(EntityDto input);

    /// <summary>
    /// Delete a Brand
    /// </summary>
    Task DeleteAsync(EntityDto input);
    
    /// <summary>
    /// Get all Categories
    /// </summary>
    Task<ListResultDto<BrandDto>> GetAllAsync();

    /// <summary>
    /// get all brand
    /// </summary>
    /// <returns></returns>

    Task<List<BrandDto>> GetListAsync();
    /// <summary>
    /// Get Paged Categories
    /// </summary>
    Task<PagedResultDto<BrandDto> > GetPagedResultAsync(PagedBrandResultRequestDto input);
}