using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Webminux.Optician;
using Webminux.Optician.Helpers;


/// <summary>
/// Provides methods to manage Groups
/// </summary>
public class SubPackageAppService : OpticianAppServiceBase, ISubPackageService
{
    private readonly IRepository<Webminux.Optician.SubPackage.SubPackage, int> _repository;
    private readonly IMediaHelperService _imageHelperService;


    /// <summary>
    /// Constructor
    /// </summary>
    public SubPackageAppService( 
        IRepository<Webminux.Optician.SubPackage.SubPackage, int> repository,
        IMediaHelperService imageHelperService
        )
    {
        _repository = repository;
        _imageHelperService = imageHelperService;


    }

    /// <summary>
    /// Create a new SubPackage
    /// </summary>
    public async Task CreateAsync(CreateSubPackageDto input)
    {

        try
        {

            MediaUploadDto uploadResult = new MediaUploadDto();
            if (!string.IsNullOrWhiteSpace(input.Base64Picture))
                uploadResult = await _imageHelperService.AddMediaAsync(input.Base64Picture);


            var SubPackage = Webminux.Optician.SubPackage.SubPackage.Create(input.PackageId, input.Contains, uploadResult.PublicId, uploadResult.Url);
            await _repository.InsertAsync(SubPackage);
            UnitOfWorkManager.Current.SaveChanges();

        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    /// <summary>
    /// Get a SubPackage
    /// </summary>
    public async Task<SubPackageDto> GetAsync(EntityDto input)
    {
         var query = _repository.GetAll();
         query = query.Where(b => b.Id == input.Id);
        if (query == null)
             throw new UserFriendlyException(OpticianConsts.ErrorMessages.NotFound);
        IQueryable<SubPackageDto> selectQuery = GetSubPackageSelectQuery(query);
        var cate = await selectQuery.FirstOrDefaultAsync();

        return cate;

    }



    /// <summary>
    /// Get a SubPackage
    /// </summary>
    public async Task<List<SubPackageDto>> GetListAsync()
    {
        var categories = await getSubPackageDtoList();
        return categories.ToList();

    }


    /// <summary>
    /// Update a Catgory
    /// </summary>
    public async Task UpdateAsync(UpdateSubPackageDto input)
    {
        var data = await _repository.GetAsync(input.Id);
        if (data == null)
            throw new UserFriendlyException(OpticianConsts.ErrorMessages.NotFound);

        ObjectMapper.Map(input, data);

     
    }

    /// <summary>
    /// Delete a SubPackage
    /// </summary>
    public async Task DeleteAsync(EntityDto input)
    {
        var SubPackageFromDB = await _repository.GetAsync(input.Id);
        if (SubPackageFromDB == null)
            throw new UserFriendlyException(OpticianConsts.ErrorMessages.NotFound);

        await _repository.DeleteAsync(SubPackageFromDB);
       
    }

    /// <summary>
    /// Get all SubPackage
    /// </summary>
    public async Task<ListResultDto<SubPackageDto>> GetAllAsync()
    {
        List<SubPackageDto> catgories = await getSubPackageDtoList();

        return new ListResultDto<SubPackageDto>(catgories);
    }

    private async Task<List<SubPackageDto>> getSubPackageDtoList()
    {
        return await (
            from cate in _repository.GetAll()
            select new SubPackageDto
            {
                Id = cate.Id,
                CreationTime = cate.CreationTime,
                CreatorUserId  = cate.CreatorUserId 


            }
        ).ToListAsync();
    }


    #region  GetPagedResult
    /// <summary>
    /// Get Paged SubPackage
    /// </summary>
    public async Task<PagedResultDto<SubPackageDto>> GetPagedResultAsync(PagedSubPackageResultRequestDto input)
    {
        var query = _repository.GetAll();
        query = ApplyFilters(input, query);
        IQueryable<SubPackageDto> selectQuery = GetSelectQuery(query);
        return await selectQuery.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
    }
   

    #endregion


    #region Private Methods
    private static IQueryable<Webminux.Optician.SubPackage.SubPackage> ApplyFilters(PagedSubPackageResultRequestDto input, IQueryable<Webminux.Optician.SubPackage.SubPackage> query)
    {
        if (!string.IsNullOrWhiteSpace(input.Keyword))
        {
            var keywords = input.Keyword.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var keyword in keywords)
            {
                query = query.Where(g => g.Contains.Contains(keyword));
            }
        }
        return query;

    }
    private static IQueryable<SubPackageDto> GetSelectQuery(IQueryable<Webminux.Optician.SubPackage.SubPackage> query)
    {
        return query.Select(b => new SubPackageDto
        {
            Id = b.Id,
            CreatorUserId = b.CreatorUserId,
            CreationTime = b.CreationTime,
            Contains = b.Contains,

            ImageUrl = b.ImageUrl
        });
    }

    private IQueryable<SubPackageDto> GetSubPackageSelectQuery(IQueryable<Webminux.Optician.SubPackage.SubPackage> query)
    {
        return query.Select(b => new SubPackageDto
        {
            Id = b.Id,
            CreatorUserId = b.CreatorUserId,
            CreationTime = b.CreationTime,
            PackageId = b.PackageId,
            ImageUrl = b.ImageUrl,
        });
    }

    #endregion
}