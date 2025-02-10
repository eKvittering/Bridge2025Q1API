using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Services;


/// <summary>
/// Interface for SubPackage AppService
/// </summary>
public interface ISubPackageService : IDomainService
{
    /// <summary>
    /// Create a new SubPackage
    /// </summary>
    Task CreateAsync(CreateSubPackageDto input);
    
    /// <summary>
    /// Update a SubPackage
    /// </summary>
    Task UpdateAsync(UpdateSubPackageDto input);

    /// <summary>
    /// Get a SubPackage
    /// </summary>
    Task<SubPackageDto> GetAsync(EntityDto input);

    /// <summary>
    /// Delete a SubPackage
    /// </summary>
    Task DeleteAsync(EntityDto input);
    
    /// <summary>
    /// Get all Categories
    /// </summary>
    Task<ListResultDto<SubPackageDto>> GetAllAsync();

    /// <summary>
    /// get list of SubPackage
    /// </summary>
    /// <returns></returns>
    Task<List<SubPackageDto>> GetListAsync();
    /// <summary>
    /// Get Paged Categories
    /// </summary>
    Task<PagedResultDto<SubPackageDto> > GetPagedResultAsync(PagedSubPackageResultRequestDto input);
}