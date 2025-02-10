using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;


/// <summary>
/// Interface for PackageType AppService
/// </summary>
public interface IPackageTypeService :IApplicationService
{
    /// <summary>
    /// Create a new PackageType
    /// </summary>
    Task CreateAsync(CreatePackageTypeDto input);
    
    /// <summary>
    /// Update a PackageType
    /// </summary>
    Task UpdateAsync(UpdatePackageTypeDto input);

    /// <summary>
    /// Get a PackageType
    /// </summary>
    Task<PackageTypeDto> GetAsync(EntityDto input);

    /// <summary>
    /// Delete a PackageType
    /// </summary>
    Task DeleteAsync(EntityDto input);
    
    /// <summary>
    /// Get all Categories
    /// </summary>
    Task<ListResultDto<PackageTypeDto>> GetAllAsync();

    /// <summary>
    /// get list of PackageType
    /// </summary>
    /// <returns></returns>
    Task<List<PackageTypeDto>> GetListAsync();
    /// <summary>
    /// Get Paged Categories
    /// </summary>
    Task<PagedResultDto<PackageTypeDto> > GetPagedResultAsync(PagedPackageTypeResultRequestDto input);
}