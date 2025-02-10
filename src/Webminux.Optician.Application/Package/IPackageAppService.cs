using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;


/// <summary>
/// Interface for Package AppService
/// </summary>
public interface IPackageService :IApplicationService
{
    /// <summary>
    /// Create a new Package
    /// </summary>
    Task CreateAsync(CreatePackageDto input);
    
    /// <summary>
    /// Update a Package
    /// </summary>
    Task UpdateAsync(UpdatePackageDto input);

    /// <summary>
    /// Get a Package
    /// </summary>
    Task<PackageDto> GetByIdAsync(int Id);

    /// <summary>
    /// Delete a Package
    /// </summary>
    Task DeleteAsync(EntityDto input);
    
    /// <summary>
    /// Get all Categories
    /// </summary>
    Task<ListResultDto<PackageDto>> GetAllAsync();

    /// <summary>
    /// get list of Package
    /// </summary>
    /// <returns></returns>
    Task<List<PackageDto>> GetListAsync();
    /// <summary>
    /// Get Paged Categories
    /// </summary>
    Task<PagedResultDto<PackageDto> > GetPagedResultAsync(PagedPackageResultRequestDto input);
}