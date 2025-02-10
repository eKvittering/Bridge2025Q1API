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
using Webminux.Optician.Categories;
using Webminux.Optician.Helpers;


/// <summary>
/// Provides methods to manage Groups
/// </summary>
public class CatogoryAppService : OpticianAppServiceBase, ICategoryService
{
    private readonly IRepository<Category, int> _repository;
  

    /// <summary>
    /// Constructor
    /// </summary>
    public CatogoryAppService( IRepository<Category, int> repository)
    {
        _repository = repository;
       
    }

    /// <summary>
    /// Create a new category
    /// </summary>
    public async Task CreateAsync(CreateCategoryDto input)
    {
        var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
      
        var category = Category.Create(tenantId, input.Name);

         await _repository.InsertAsync(category);
        UnitOfWorkManager.Current.SaveChanges();
        
    }

    /// <summary>
    /// Get a category
    /// </summary>
    public async Task<CategoryDto> GetAsync(EntityDto input)
    {
         var query = _repository.GetAll();
         query = query.Where(b => b.Id == input.Id);
        if (query == null)
             throw new UserFriendlyException(OpticianConsts.ErrorMessages.CategoryNotFound);
        IQueryable<CategoryDto> selectQuery = GetCategorySelectQuery(query);
        var cate = await selectQuery.FirstOrDefaultAsync();

        return cate;

    }



    /// <summary>
    /// Get a category
    /// </summary>
    public async Task<List<CategoryDto>> GetListAsync()
    {
        var categories = await getCategoryDtoList();
        return categories.Where(s=>s.IsDeactive==false).ToList();

    }


    /// <summary>
    /// Update a Catgory
    /// </summary>
    public async Task UpdateAsync(UpdateCategoryDto input)
    {
        var data = await _repository.GetAsync(input.Id);
        if (data == null)
            throw new UserFriendlyException(OpticianConsts.ErrorMessages.CategoryNotFound);

        ObjectMapper.Map(input, data);

     
    }

    /// <summary>
    /// Delete a Category
    /// </summary>
    public async Task DeleteAsync(EntityDto input)
    {
        var categoryFromDB = await _repository.GetAsync(input.Id);
        if (categoryFromDB == null)
            throw new UserFriendlyException(OpticianConsts.ErrorMessages.CategoryNotFound);

        await _repository.DeleteAsync(categoryFromDB);
       
    }

    /// <summary>
    /// Get all Category
    /// </summary>
    public async Task<ListResultDto<CategoryDto>> GetAllAsync()
    {
        List<CategoryDto> catgories = await getCategoryDtoList();

        return new ListResultDto<CategoryDto>(catgories);
    }

    private async Task<List<CategoryDto>> getCategoryDtoList()
    {
        return await (
            from cate in _repository.GetAll()
            select new CategoryDto
            {
                Id = cate.Id,
                Name = cate.Name,
                ParentCategoryId = cate.ParentCategoryId,
                CreatorUserId = cate.CreatorUserId,
                CreationTime = cate.CreationTime,
                IsDeactive = cate.IsDeactive,


            }
        ).ToListAsync();
    }


    #region  GetPagedResult
    /// <summary>
    /// Get Paged Category
    /// </summary>
    public async Task<PagedResultDto<CategoryDto>> GetPagedResultAsync(PagedCategoryResultRequestDto input)
    {
        var query = _repository.GetAll();
        query = ApplyFilters(input, query);
        IQueryable<CategoryDto> selectQuery = GetSelectQuery(query);
        return await selectQuery.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
    }
   

    #endregion


    #region Private Methods
    private static IQueryable<Category> ApplyFilters(PagedCategoryResultRequestDto input, IQueryable<Category> query)
    {
        if (string.IsNullOrWhiteSpace(input.Keyword) == false)
            query = query.Where(g => g.Name.Contains(input.Keyword));
        return query;
    }
    private static IQueryable<CategoryDto> GetSelectQuery(IQueryable<Category> query)
    {
        return query.Select(g => new CategoryDto
        {
            Id = g.Id,
            Name = g.Name,
            CreatorUserId = g.CreatorUserId,
            CreationTime = g.CreationTime,
            IsDeactive = g.IsDeactive,
            ParentCategoryId = g.ParentCategoryId
        });
    }

    private IQueryable<CategoryDto> GetCategorySelectQuery(IQueryable<Category> query)
    {
        return query.Select(b => new CategoryDto
        {
            Id = b.Id,
            Name = b.Name,
            IsDeactive = b.IsDeactive,
            CreationTime = b.CreationTime,
            CreatorUserId = b.CreatorUserId,
            ParentCategoryId = b.ParentCategoryId,


        });
    }

    #endregion
}