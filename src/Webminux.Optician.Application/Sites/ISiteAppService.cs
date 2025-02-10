using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Webminux.Optician.Application.Sites.Dtos;

/// <summary>
/// Interface for Site AppService
/// </summary>
public interface ISiteAppService :IApplicationService
{
    /// <summary>
    /// Create a new Site
    /// </summary>
    Task CreateAsync(CreateSiteDto input);
    
    /// <summary>
    /// Update a Site
    /// </summary>
    Task UpdateAsync(UpdateSiteDto input);
    /// <summary>
    /// Get a Site
    /// </summary>
    Task<SiteDetailDto> GetAsync(EntityDto input);
    /// <summary>
    /// Delete a Site
    /// </summary>
    Task DeleteAsync(EntityDto input);
    /// <summary>
    /// Get all Sites
    /// </summary>
    Task<ListResultDto<SiteDto>> GetAllAsync();
    /// <summary>
    /// Get Paged Sites
    /// </summary>
    Task<PagedResultDto<SiteDto> > GetPagedResultAsync(PagedSiteResultRequestDto input);

}