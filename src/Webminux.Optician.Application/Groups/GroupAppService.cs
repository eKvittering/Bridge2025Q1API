using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Webminux.Optician;
using Webminux.Optician.Application;
using Webminux.Optician.Application.Groups.Dtos;
using Webminux.Optician.Helpers;

/// <summary>
/// Provides methods to manage Groups
/// </summary>
public class GroupAppService : OpticianAppServiceBase, IGroupAppService
{
    private readonly IRepository<Group, int> _repository;
    private readonly IRepository<EmployeeGroup> _userGroupRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    public GroupAppService(
        IRepository<Group, int> repository,
        IRepository<EmployeeGroup> userGroupRepository)
    {
        _repository = repository;
        _userGroupRepository = userGroupRepository;
    }

    /// <summary>
    /// Create a new Group
    /// </summary>
    public async Task CreateAsync(CreateGroupDto input)
    {
        var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
        Group g = new Group();
        var group = g.Create(tenantId, input.Name);

        var groupFromDb = await _repository.InsertAsync(group);
        UnitOfWorkManager.Current.SaveChanges();
        await InsertCustomerGroup(tenantId, groupFromDb.Id, input.UserIds);
    }


    public async Task<GroupDto> GetGroupByName(string input)
    {
        var query = from g in _repository.GetAll()
                    where g.Name == input
                    select new GroupDto
                    {
                        Id = g.Id,
                        Name = g.Name,
                    };

        var group = await query.FirstOrDefaultAsync();

        return group;
    }
    /// <summary>
    /// Get a Group
    /// </summary>
    public async Task<GroupDetailDto> GetAsync(EntityDto input)
    {
        var query = from g in _repository.GetAll()
                    join cg in _userGroupRepository.GetAll() on g.Id equals cg.GroupId
                    where g.Id == input.Id
                    select new
                    {
                        Id = g.Id,
                        Name = g.Name,
                        Users = new UserListDto { Id = cg.User.Id, Name = cg.User.FullName, EmailAddress = cg.User.EmailAddress,UserTypeName= cg.User.UserType.Name,CreatorUserId = cg.User.CreatorUserId }
                    };

        var customerGroups = await query.ToListAsync();

        if (customerGroups.Count() == 0)
            throw new UserFriendlyException(OpticianConsts.ErrorMessages.GroupNotFound);

        return new GroupDetailDto
        {
            Id = customerGroups.FirstOrDefault().Id,
            Name = customerGroups.FirstOrDefault().Name,
            Users = customerGroups.Select(c => c.Users).ToList()
        };
    }



    /// <summary>
    /// Update a Group
    /// </summary>
    public async Task UpdateAsync(UpdateGroupDto input)
    {
        var groupFromDb = await _repository.GetAsync(input.Id);
        if (groupFromDb == null)
            throw new UserFriendlyException(OpticianConsts.ErrorMessages.GroupNotFound);

        //ObjectMapper.Map(input, groupFromDb);
        await DeleteOldCustomerGroup(input.Id);
        await InsertCustomerGroup(AbpSession.TenantId ?? OpticianConsts.DefaultTenantId, groupFromDb.Id, input.UserIds);
    }

    /// <summary>
    /// Delete a Group
    /// </summary>
    public async Task DeleteAsync(EntityDto input)
    {
        var groupFromDb = await _repository.GetAsync(input.Id);
        if (groupFromDb == null)
            throw new UserFriendlyException(OpticianConsts.ErrorMessages.GroupNotFound);

        await _repository.DeleteAsync(groupFromDb);
        await DeleteOldCustomerGroup(input.Id);
    }

    /// <summary>
    /// Get all Groups
    /// </summary>
    public async Task<ListResultDto<GroupDto>> GetAllAsync()
    {
        var groups = await (
            from g in _repository.GetAll()
            select new GroupDto
            {
                Id = g.Id,
                Name = g.Name
            }
        ).ToListAsync();

        return new ListResultDto<GroupDto>(groups);
    }

    /// <summary>
    /// Get Customer Groups
    /// </summary>
    public async Task<ListResultDto<GroupDto>> GetCustomerGroupsAsync(EntityDto input)
    {
        var query = from g in _repository.GetAll()
                    join cg in _userGroupRepository.GetAll() on g.Id equals cg.GroupId
                    where cg.User.Id == input.Id
                    select new GroupDto
                    {
                        Id = (int)cg.User.Id,
                        Name = g.Name,
                        CreationTime = g.CreationTime,
                        CreatorUserId = g.CreatorUserId
                    };

        var customerGroups = await query.ToListAsync();

        return new ListResultDto<GroupDto>(customerGroups);
    }

    /// <summary>
    /// Get Customers added in particular group
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<ListResultDto<UserListDto>> GetGroupUsers(EntityDto input)
    {
        var query = _userGroupRepository.GetAll();
        query = query.Where(customerGroup => customerGroup.GroupId == input.Id);
        var users = await query.
            Select(userGroup => new UserListDto
            {
                Id= userGroup.User.Id,
                Name = userGroup.User.FullName,
                EmailAddress = userGroup.User.EmailAddress,
            }).ToListAsync();

        return new ListResultDto<UserListDto>(users);
    }

    public async Task<List<long?>> CheckEmployeeGroups(long? Id)
    {
        var query = _userGroupRepository.GetAll();
        query = query.Where(customerGroup => customerGroup.EmployeeId == Id);
        List<long?> groupIds = await query.Select(userGroup => (long?)userGroup.GroupId).ToListAsync();

        return groupIds;
    }

    #region  GetPagedResult
    /// <summary>
    /// Get Paged Groups
    /// </summary>
    public async Task<PagedResultDto<GroupDto>> GetPagedResultAsync(PagedGroupResultRequestDto input)
    {
        var query = _repository.GetAll();
        query = ApplyFilters(input, query);
        IQueryable<GroupDto> selectQuery = GetSelectQuery(query);
        return await selectQuery.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
    }
    private static IQueryable<GroupDto> GetSelectQuery(IQueryable<Group> query)
    {
        return query.Select(g => new GroupDto
        {
            Id = g.Id,
            Name = g.Name,
            CreatorUserId = g.CreatorUserId,
            CreationTime = g.CreationTime
        });
    }

    private static IQueryable<Group> ApplyFilters(PagedGroupResultRequestDto input, IQueryable<Group> query)
    {
        if (string.IsNullOrWhiteSpace(input.Keyword) == false)
            query = query.Where(g => g.Name.Contains(input.Keyword));
        return query;
    }

    #endregion


    #region Private Methods
    private async Task InsertCustomerGroup(int tenantId, int groupId, ICollection<int> customerIds)
    {
        foreach (var customer in customerIds)
        {
            var GC = new EmployeeGroup();
            var customerGroup = GC.Create(tenantId, customer, groupId);
            await _userGroupRepository.InsertAsync(customerGroup);
        }
    }

    /// <summary>
    /// Delete old customer group
    /// </summary>
    private async Task DeleteOldCustomerGroup(int groupId)
    {
        using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
        {
            var customerGroups = await _userGroupRepository.GetAllListAsync(cg => cg.GroupId == groupId);
            foreach (var customerGroup in customerGroups)
            {
                await _userGroupRepository.DeleteAsync(customerGroup);
            }
        }
    }

    #endregion
}