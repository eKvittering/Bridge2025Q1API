using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Webminux.Optician;
using Webminux.Optician.Brands;
using Webminux.Optician.Helpers;


/// <summary>
/// Provides methods to manage Groups
/// </summary>
public class BrandAppService : OpticianAppServiceBase, IBrandService
{
    private readonly IRepository<Brand, int> _repository;
  

    /// <summary>
    /// Constructor
    /// </summary>
    public BrandAppService( IRepository<Brand, int> repository)
    {
        _repository = repository;
       
    }

    /// <summary>
    /// Create a new Brand
    /// </summary>
    public async Task CreateAsync(CreateBrandDto input)
    {
        var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
      
        var brand = Brand.Create(tenantId, input.Name);

         await _repository.InsertAsync(brand);
        UnitOfWorkManager.Current.SaveChanges();
        
    }

    /// <summary>
    /// Get a Brand
    /// </summary>
    public async Task<BrandDto> GetAsync(EntityDto input)
    {
         var query = _repository.GetAll();
         query = query.Where(b => b.Id == input.Id);
        if (query == null)
             throw new UserFriendlyException(OpticianConsts.ErrorMessages.BrandNotFound);
        IQueryable<BrandDto> selectQuery = GetBrandSelectQuery(query);
        var cate = await selectQuery.FirstOrDefaultAsync();

        return cate;

    }



    /// <summary>
    /// Get a Brand
    /// </summary>
    public async Task<List<BrandDto>> GetListAsync()
    {
        var categories = await getBrandDtoList();
        return categories.Where(s=>s.IsDeactive==false).ToList();

    }


    /// <summary>
    /// Update a Catgory
    /// </summary>
    public async Task UpdateAsync(UpdateBrandDto input)
    {
        var data = await _repository.GetAsync(input.Id);
        if (data == null)
            throw new UserFriendlyException(OpticianConsts.ErrorMessages.BrandNotFound);

        ObjectMapper.Map(input, data);

     
    }

    /// <summary>
    /// Delete a Brand
    /// </summary>
    public async Task DeleteAsync(EntityDto input)
    {
        var BrandFromDB = await _repository.GetAsync(input.Id);
        if (BrandFromDB == null)
            throw new UserFriendlyException(OpticianConsts.ErrorMessages.BrandNotFound);

        await _repository.DeleteAsync(BrandFromDB);
       
    }

    /// <summary>
    /// Get all Brand
    /// </summary>
    public async Task<ListResultDto<BrandDto>> GetAllAsync()
    {
        List<BrandDto> catgories = await getBrandDtoList();

        return new ListResultDto<BrandDto>(catgories);
    }

    private async Task<List<BrandDto>> getBrandDtoList()
    {
        return await (
            from cate in _repository.GetAll()
            select new BrandDto
            {
                Id = cate.Id,
                Name = cate.Name,
                ParentBrandId = cate.ParentBrandId,
                CreatorUserId = cate.CreatorUserId,
                CreationTime = cate.CreationTime,
                IsDeactive = cate.IsDeactive,


            }
        ).ToListAsync();
    }


    #region  GetPagedResult
    /// <summary>
    /// Get Paged Brand
    /// </summary>
    public async Task<PagedResultDto<BrandDto>> GetPagedResultAsync(PagedBrandResultRequestDto input)
    {
        var query = _repository.GetAll();
        query = ApplyFilters(input, query);
        IQueryable<BrandDto> selectQuery = GetSelectQuery(query);
        return await selectQuery.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
    }
   

    #endregion


    #region Private Methods
    private static IQueryable<Brand> ApplyFilters(PagedBrandResultRequestDto input, IQueryable<Brand> query)
    {
        if (string.IsNullOrWhiteSpace(input.Keyword) == false)
            query = query.Where(g => g.Name.Contains(input.Keyword));
        return query;
    }
    private static IQueryable<BrandDto> GetSelectQuery(IQueryable<Brand> query)
    {
        return query.Select(g => new BrandDto
        {
            Id = g.Id,
            Name = g.Name,
            CreatorUserId = g.CreatorUserId,
            CreationTime = g.CreationTime,
            IsDeactive = g.IsDeactive,
            ParentBrandId = g.ParentBrandId
        });
    }

    private IQueryable<BrandDto> GetBrandSelectQuery(IQueryable<Brand> query)
    {
        return query.Select(b => new BrandDto
        {
            Id = b.Id,
            Name = b.Name,
            IsDeactive = b.IsDeactive,
            CreationTime = b.CreationTime,
            CreatorUserId = b.CreatorUserId,
            ParentBrandId = b.ParentBrandId,


        });
    }

    #endregion
}