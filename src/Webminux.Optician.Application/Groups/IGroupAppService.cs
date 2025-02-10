using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Webminux.Optician.Application;
using Webminux.Optician.Application.Groups.Dtos;

/// <summary>
/// Interface for Group AppService
/// </summary>
public interface IGroupAppService :IApplicationService
{
    /// <summary>
    /// Create a new Group
    /// </summary>
    Task CreateAsync(CreateGroupDto input);
    
    /// <summary>
    /// Update a Group
    /// </summary>
    Task UpdateAsync(UpdateGroupDto input);
    /// <summary>
    /// Get a Group
    /// </summary>
    Task<GroupDetailDto> GetAsync(EntityDto input);
    /// <summary>
    /// Delete a Group
    /// </summary>
    Task DeleteAsync(EntityDto input);
    /// <summary>
    /// Get all Groups
    /// </summary>
    Task<ListResultDto<GroupDto>> GetAllAsync();
    /// <summary>
    /// Get Paged Groups
    /// </summary>
    Task<PagedResultDto<GroupDto> > GetPagedResultAsync(PagedGroupResultRequestDto input);

    Task<ListResultDto<UserListDto>> GetGroupUsers(EntityDto input);
    Task<ListResultDto<GroupDto>> GetCustomerGroupsAsync(EntityDto input);

    Task<List<long?>> CheckEmployeeGroups(long? Id);

    Task<GroupDto> GetGroupByName(string input);
}