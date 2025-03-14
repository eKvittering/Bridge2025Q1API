using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Webminux.Optician;
using Webminux.Optician.Core.Helpers;
using Webminux.Optician.Helpers;
using Webminux.Optician.Invoices.dtos;
using static Webminux.Optician.OpticianConsts;

/// <summary>
/// Provide methods for Activity Invites.
/// </summary>
public class InviteAppService : OpticianAppServiceBase, IInviteAppService
{
    private readonly IInviteManager _inviteManager;
    private readonly IRepository<CustomerGroup> _customerGroupRepository;

    /// <summary>
    /// Constructor.
    /// </summary>
    public InviteAppService(
        IInviteManager inviteManager,
        IRepository<CustomerGroup> customerGroupRepository
        )
    {
        _inviteManager = inviteManager;
        _customerGroupRepository = customerGroupRepository;
    }

    #region CreateAsync
    /// <summary>
    /// Create Invite for all customers in group.
    /// </summary>
    public async Task CreateAsync(CreateInviteDto input)
    {
        var customersInGroup = await _customerGroupRepository.GetAllListAsync(x => x.GroupId == input.GroupId);
        ValidateCustomerGroupResult(customersInGroup);
        await ValidateInviteAlreadyCreated(input);

        await _inviteManager.CreateAsync(GetInvites(input, customersInGroup));


    }

    private async Task ValidateInviteAlreadyCreated(CreateInviteDto input)
    {
        var invite = await _inviteManager.GetAll().FirstOrDefaultAsync(i => i.GroupId == input.GroupId && i.ActivityId == input.ActivityId);

        if (invite == null)
            return;

        throw new UserFriendlyException(L("Invite already created"));
    }

    private static void ValidateCustomerGroupResult(List<CustomerGroup> customersInGroup)
    {
        if (customersInGroup.Count() == 0)
            throw new UserFriendlyException(OpticianConsts.ErrorMessages.GroupNotFound);
    }

    private List<Invite> GetInvites(CreateInviteDto input, List<CustomerGroup> customersInGroup)
    {
        var invites = new List<Invite>();

        foreach (var customer in customersInGroup)
        {
            invites.Add(Invite.Create(AbpSession.TenantId ?? OpticianConsts.DefaultTenantId, customer.CustomerId, input.GroupId, input.ActivityId));
        }

        return invites;
    }
    #endregion


    #region Get
    /// <summary>
    /// Get Invite on basis of Id.
    /// </summary>
    public async Task<InviteDto> GetAsync(GetInviteInputDto input)
    {
        var query = _inviteManager.GetAll();
        query = query.Where(i => i.ActivityId == input.ActivityId && i.GroupId == input.GroupId);
        var selectQuery = GetSelectQuery(query);
        return await selectQuery.FirstOrDefaultAsync();
    }

    /// <summary>
    /// Get invites against specific customer and activity
    /// </summary>
    /// <param name="input">Input object contain customer or group and activity Id's</param>
    /// <returns>Invite</returns>
    public async Task<InviteDto> GetCustomerInviteAsync(GetInviteInputDto input)
    {
        var query = _inviteManager.GetAll();
        query = query.Where(i => i.ActivityId == input.ActivityId && i.UserId == input.CustomerId);
        var selectQuery = GetSelectQueryForCustomerInvite(query);
        return await selectQuery.FirstOrDefaultAsync();
    }


    #endregion

    #region Get All
    /// <summary>
    /// Get Invites against group of customers
    /// </summary>
    public async Task<ListResultDto<InviteDto>> GetGroupInvitesAsync(EntityDto input)
    {

        var query = _inviteManager.GetAll();
        query = query.Where(x => x.GroupId == input.Id);
        var selectQuery = GetSelectQuery(query);
        var invites = await selectQuery.ToListAsync();

        return new ListResultDto<InviteDto>(invites);
    }

    /// <summary>
    /// Get invites against customer
    /// </summary>
    /// <param name="input">Object contains Id of customer</param>
    /// <returns>List of invites</returns>
    public async Task<ListResultDto<InviteDto>> GetCustomerInvitesAsync(EntityDto input)
    {

        var query = _inviteManager.GetAll();
        query = query.Where(x => x.UserId == input.Id);
        var selectQuery = GetSelectQueryForCustomerInvite(query);
        var invites = await selectQuery.ToListAsync();

        return new ListResultDto<InviteDto>(invites);
    }
    #endregion
    /// <summary>
    /// Get Activity Invites.
    /// </summary>
    public async Task<ListResultDto<InviteDto>> GetActivityInvitesAsync(EntityDto input)
    {
        var query = _inviteManager.GetAll();
        query = query.Where(i => i.ActivityId == input.Id);
        var selectQuery = GetSelectQueryForActivityInvites(query);
        var invites = await selectQuery.ToListAsync();

        return new ListResultDto<InviteDto>(invites);
    }
    private static IQueryable<Invite> ApplyFilters(PagedInviteResultRequestDto input, IQueryable<Invite> query)
    {
        if (input.CustomerId.HasValue)
            query = query.Where(x => x.UserId == input.CustomerId.Value);
        if (string.IsNullOrWhiteSpace(input.Keyword) == false)
            query = query.Where(x => x.Activity.Name.Contains(input.Keyword));
        return query;
    }

    /// <summary>
    /// Get Invite Response List.
    /// </summary>
    public ListResultDto<NameValueDto<int>> GetInviteResponsesAsync()
    {
        var inviteResponses = Enum.GetValues(typeof(InviteResponse)).Cast<InviteResponse>().Select(x => new NameValueDto<int>
        {
            Name = x.ToString(),
            Value = (int)x
        }).ToList();

        return new ListResultDto<NameValueDto<int>>(inviteResponses);
    }

    /// <summary>
    /// Update Invite Response.
    /// </summary>
    public async Task UpdateInviteResponseAsync(UpdateInviteResponseInputDto input)
    {
        await _inviteManager.UpdateInviteResponseAsync(input.Id, (InviteResponse)input.Response);
    }

    #region helpers 
    private IQueryable<InviteDto> GetSelectQuery(IQueryable<Invite> query)
    {
        var selectQuery = from invite in query
                          group invite by invite.ActivityId into activityInvite
                          select new InviteDto
                          {
                              Id = activityInvite.First().Id,
                              GroupId = activityInvite.First().GroupId,
                              GroupName = activityInvite.First().Group == null ? string.Empty : activityInvite.First().Group.Name,

                              ActivityId = activityInvite.First().ActivityId,
                              Activity = new ActivityDto
                              {
                                  Id = activityInvite.First().Activity.Id,
                                  Name = activityInvite.First().Activity.Name,
                                  //CustomerId = activityInvite.First().Activity.CustomerId,
                                  ActivityTypeId = activityInvite.First().Activity.ActivityTypeId,
                                  ActivityArtId = activityInvite.First().Activity.ActivityArtId,
                                  Date = activityInvite.First().Activity.Date,
                                  FollowUpDate = activityInvite.First().Activity.FollowUpDate,
                                  FollowUpTypeId = activityInvite.First().Activity.FollowUpTypeId,
                                  EmployeeId = activityInvite.First().Activity.UserId,
                                  IsFollowUp = activityInvite.First().Activity.IsFollowUp,
                                  FollowUpByEmployeeId = activityInvite.First().Activity.FollowUpByEmployeeId,
                                  TenantId = activityInvite.First().TenantId
                              },
                              CreationTime = activityInvite.First().CreationTime,
                              CreatorUserId = activityInvite.First().CreatorUserId,
                              //Responses = activityInvite.Select(x => new CustomerResponseDto
                              //{
                              //    Id = x.CustomerId.Value,
                              //    Name = x.Customer.User.FullName,
                              //    Response = (int)x.Response
                              //}).ToList()
                          };
        return selectQuery;
    }

    private IQueryable<InviteDto> GetSelectQueryForActivityInvites(IQueryable<Invite> query)
    {
        var selectQuery = from invite in query
                          group invite by invite.GroupId into groupInvite
                          select new InviteDto
                          {
                              Id = groupInvite.First().Id,
                              GroupId = groupInvite.First().GroupId,
                              GroupName = groupInvite.First().Group == null ? string.Empty : groupInvite.First().Group.Name,
                              ActivityId = groupInvite.First().ActivityId,
                              Activity = new ActivityDto
                              {
                                  Id = groupInvite.First().Activity.Id,
                                  Name = groupInvite.First().Activity.Name,
                                  CustomerId = groupInvite.First().Activity.CustomerId,
                                  ActivityTypeId = groupInvite.First().Activity.ActivityTypeId,
                                  ActivityArtId = groupInvite.First().Activity.ActivityArtId,
                                  Date = groupInvite.First().Activity.Date,
                                  FollowUpDate = groupInvite.First().Activity.FollowUpDate,
                                  FollowUpTypeId = groupInvite.First().Activity.FollowUpTypeId,
                                  EmployeeId = groupInvite.First().Activity.UserId,
                                  IsFollowUp = groupInvite.First().Activity.IsFollowUp,
                                  FollowUpByEmployeeId = groupInvite.First().Activity.FollowUpByEmployeeId,
                                  TenantId = groupInvite.First().TenantId
                              },
                              CreationTime = groupInvite.First().CreationTime,
                              CreatorUserId = groupInvite.First().CreatorUserId,
                              Responses = groupInvite.Select(x => new CustomerResponseDto
                              {
                                  Id = x.UserId.Value,
                                  Name = x.User.FullName,
                                  Response = (int)x.Response
                              }).ToList()
                          };
        return selectQuery;
    }

    private IQueryable<InviteDto> GetSelectQueryForCustomerInvite(IQueryable<Invite> query)
    {
        var selectQuery = from invite in query
                          select new InviteDto
                          {
                              Id = invite.Id,
                              CustomerId = invite.UserId.Value,
                              ActivityId = invite.ActivityId,
                              Activity = new ActivityDto
                              {
                                  Id = invite.Activity.Id,
                                  Name = invite.Activity.Name,
                                  CustomerId = invite.Activity.CustomerId,
                                  ActivityTypeId = invite.Activity.ActivityTypeId,
                                  ActivityArtId = invite.Activity.ActivityArtId,
                                  Date = invite.Activity.Date,
                                  FollowUpDate = invite.Activity.FollowUpDate,
                                  FollowUpTypeId = invite.Activity.FollowUpTypeId,
                                  EmployeeId = invite.Activity.UserId,
                                  IsFollowUp = invite.Activity.IsFollowUp,
                                  FollowUpByEmployeeId = invite.Activity.FollowUpByEmployeeId,
                                  TenantId = invite.TenantId
                              },
                              CreationTime = invite.CreationTime,
                              CreatorUserId = invite.CreatorUserId,
                              Responses = new List<CustomerResponseDto>{ new CustomerResponseDto
                              {
                                  Id = invite.UserId.Value,
                                  Name = invite.User.FullName,
                                  Response = (int)invite.Response
                              } }
                          };
        return selectQuery;
    }
    #endregion
}