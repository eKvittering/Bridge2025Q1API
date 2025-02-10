using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Webminux.Optician.Invoices.dtos;

/// <summary>
/// Interface for Invite AppService
/// </summary>
public interface IInviteAppService : IApplicationService
{
    /// <summary>
    /// Get Invite on basis of Id.
    /// </summary>    
    Task<InviteDto> GetAsync(GetInviteInputDto input);

    /// <summary>
    /// Get Paged Invites.
    /// </summary>
    Task<ListResultDto<InviteDto>> GetGroupInvitesAsync(EntityDto input);

    /// <summary>
    /// Create a new Invite
    /// </summary>
    Task CreateAsync(CreateInviteDto input);

    /// <summary>
    /// Return Invite Response List
    /// </summary>
    ListResultDto<NameValueDto<int>> GetInviteResponsesAsync();

    /// <summary>
    /// Update Invite Response
    /// </summary>
    Task UpdateInviteResponseAsync(UpdateInviteResponseInputDto input);

    /// <summary>
    /// Get Activity Invites.
    /// </summary>
    Task<ListResultDto<InviteDto>> GetActivityInvitesAsync(EntityDto input);

    /// <summary>
    /// Get invite against customer
    /// </summary>
    /// <param name="input">Object contains group id or customer id and activity id</param>
    /// <returns>Invite</returns>
    Task<InviteDto> GetCustomerInviteAsync(GetInviteInputDto input);

    /// <summary>
    /// Get invites against customer
    /// </summary>
    /// <param name="input">Object contains Id of customer</param>
    /// <returns>List of invites</returns>
    Task<ListResultDto<InviteDto>> GetCustomerInvitesAsync(EntityDto input);

}