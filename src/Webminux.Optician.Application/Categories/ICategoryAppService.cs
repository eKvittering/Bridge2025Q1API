using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;


/// <summary>
/// Interface for Category AppService
/// </summary>
public interface ICategoryService :IApplicationService
{
    /// <summary>
    /// Create a new Category
    /// </summary>
    Task CreateAsync(CreateCategoryDto input);
    
    /// <summary>
    /// Update a Category
    /// </summary>
    Task UpdateAsync(UpdateCategoryDto input);

    /// <summary>
    /// Get a Category
    /// </summary>
    Task<CategoryDto> GetAsync(EntityDto input);

    /// <summary>
    /// Delete a Category
    /// </summary>
    Task DeleteAsync(EntityDto input);
    
    /// <summary>
    /// Get all Categories
    /// </summary>
    Task<ListResultDto<CategoryDto>> GetAllAsync();

    /// <summary>
    /// get list of category
    /// </summary>
    /// <returns></returns>
    Task<List<CategoryDto>> GetListAsync();
    /// <summary>
    /// Get Paged Categories
    /// </summary>
    Task<PagedResultDto<CategoryDto> > GetPagedResultAsync(PagedCategoryResultRequestDto input);
}