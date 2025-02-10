using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Webminux.Optician;
using Webminux.Optician.PackageType;
using Webminux.Optician.Users;


/// <summary>
/// Provides methods to manage Groups
/// </summary>
public class PackageTypeAppService : OpticianAppServiceBase, IPackageTypeService
{
    private readonly IRepository<PackageType, int> _repositoryPackageType;
    private readonly IUserAppService _userAppService;
    private readonly IActivityAppService _activityService;

    /// <summary>
    /// Constructor
    /// </summary>
    public PackageTypeAppService(
        IUserAppService userAppService,
        IRepository<PackageType, int> repository,
        IActivityAppService activityRepository
        )
    {
        _repositoryPackageType = repository;
        _userAppService = userAppService;
        _activityService = activityRepository;

    }

    /// <summary>
    /// Create a new PackageType
    /// </summary>
    public async Task CreateAsync(CreatePackageTypeDto input)
    {

        try
        {
            var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;

            var PackageType = Webminux.Optician.PackageType.PackageType.Create(tenantId, input.Name, input.FollowUpTypeId,input.senderTypeId, input.UserId, input.FaultId);
            await _repositoryPackageType.InsertAsync(PackageType);
            UnitOfWorkManager.Current.SaveChanges();
        }
        catch (Exception ex)
        {

            throw ex;
        }
        
    }

    /// <summary>
    /// Get a PackageType
    /// </summary>
    public async Task<PackageTypeDto> GetAsync(EntityDto input)
    {
         var query = _repositoryPackageType.GetAll();
         query = query.Where(b => b.Id == input.Id);
        if (query == null)
             throw new UserFriendlyException(OpticianConsts.ErrorMessages.NotFound);
        IQueryable<PackageTypeDto> selectQuery = GetPackageTypeSelectQuery(query);
        var cate = await selectQuery.FirstOrDefaultAsync();

        return cate;

    }



    /// <summary>
    /// Get a PackageType
    /// </summary>
    public async Task<List<PackageTypeDto>> GetListAsync()
    {
        var packageTypes = await GetPackageTypeDtoList();
        return packageTypes.ToList();

    }


    /// <summary>
    /// Update a Catgory
    /// </summary>
    public async Task UpdateAsync(UpdatePackageTypeDto input)
    {
        var data = await _repositoryPackageType.GetAsync(input.Id);
        if (data == null)
            throw new UserFriendlyException(OpticianConsts.ErrorMessages.NotFound);

        data.FollowUpTypeId = input.FollowUpTypeId;
        data.UserId = input.UserId;
        data.Name = input.Name;       
        data.senderTypeId = input.senderTypeId;
        await _repositoryPackageType.UpdateAsync(data);
        // Save the package changes
        await UnitOfWorkManager.Current.SaveChangesAsync();


    }

    /// <summary>
    /// Delete a PackageType
    /// </summary>
    public async Task DeleteAsync(EntityDto input)
    {
        var PackageTypeFromDB = await _repositoryPackageType.GetAsync(input.Id);
        if (PackageTypeFromDB == null)
            throw new UserFriendlyException(OpticianConsts.ErrorMessages.NotFound);

        await _repositoryPackageType.DeleteAsync(PackageTypeFromDB);
       
    }

    /// <summary>
    /// Get all PackageType
    /// </summary>
    public async Task<ListResultDto<PackageTypeDto>> GetAllAsync()
    {
        List<PackageTypeDto> data = await GetPackageTypeDtoList();

        return new ListResultDto<PackageTypeDto>(data);
    }




    private async Task<List<PackageTypeDto>> GetPackageTypeDtoList()
    {
        var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
        // Load data into memory
        var packages = await _repositoryPackageType.GetAll()
                          .Where(p => p.TenantId == tenantId)
                          .ToListAsync();
        var users = await _userAppService.GetAllUsers();
        var followUpTypes = await _activityService.GetAllActivityTypesAsync();

        List<PackageTypeDto> packageTypeDtos = packages.Select(package =>
        {
            var user = users.Items.FirstOrDefault(u => u.Id == package.UserId);
            var followUp = followUpTypes.Items.FirstOrDefault(f => f.Id == package.FollowUpTypeId);

            return new PackageTypeDto
            {
                Id = package.Id,
                Name = package.Name,
                UserId = package.UserId,
                FaultId = package.FaultId,
                FollowUpTypeId = package.FollowUpTypeId,
                CreationTime = package.CreationTime,
                CreatorUserId = package.CreatorUserId,
                TenantId = package.TenantId,
                UserFullName = user != null ? user.Name + " " + user.Surname : null,
                UserTypeId = user?.UserTypeId ?? 0,
                FollowUpTypeName = followUp?.Name
            };
        }).ToList();

        return packageTypeDtos;
    }




    #region  GetPagedResult
    /// <summary>
    /// Get Paged PackageType
    /// </summary>
    public async Task<PagedResultDto<PackageTypeDto>> GetPagedResultAsync(PagedPackageTypeResultRequestDto input)
    {
        try
        {
            //var query = _repositoryPackageType.GetAll(); AsQueryable

            var data = await GetPackageTypeDtoList();
            var query = data.AsQueryable();
            query = ApplyFilters(input, query);
            IQueryable<PackageTypeDto> selectQuery = GetSelectQuery(query);

            var totalCount = query.Count();
            var result =  query.OrderByDescending(q => q.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return new PagedResultDto<PackageTypeDto>(totalCount, result);
          //  return  selectQuery.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
   

    #endregion


    #region Private Methods
    private static IQueryable<PackageTypeDto> ApplyFilters(PagedPackageTypeResultRequestDto input, IQueryable<PackageTypeDto> query)
    {
        if (string.IsNullOrWhiteSpace(input.Keyword) == false)
            query = query.Where(g => g.Name.Contains(input.Keyword));
        return query;
    }
    private static IQueryable<PackageTypeDto> GetSelectQuery(IQueryable<PackageTypeDto> query)
    {
        return query.Select(b => new PackageTypeDto
        {
            Id = b.Id,
            Name = b.Name,
            UserId = b.UserId,
            CreatorUserId = b.CreatorUserId,
            CreationTime = b.CreationTime,
            FaultId = b.FaultId,
            FollowUpTypeId = b.FollowUpTypeId,
            FollowUpTypeName = b.FollowUpTypeName,
            TenantId = b.TenantId,
            UserFullName = b.UserFullName,
            UserTypeId = b.UserTypeId,
            
        });
    }

    private IQueryable<PackageTypeDto> GetPackageTypeSelectQuery(IQueryable<PackageType> query)
    {
        return query.Select(b => new PackageTypeDto
        {
            Id = b.Id,
            Name = b.Name,
            UserId = b.UserId,
            CreatorUserId = b.CreatorUserId,
            CreationTime = b.CreationTime,
            senderTypeId = b.senderTypeId,
            FollowUpTypeId= b.FollowUpTypeId,

        });
    }

    #endregion
}