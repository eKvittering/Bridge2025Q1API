using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webminux.Optician;
using Webminux.Optician.Application.Sites.Dtos;
using Webminux.Optician.Helpers;
using Webminux.Optician.Sites;

/// <summary>
/// Provides methods to manage Site
/// </summary>
public class SiteAppService : OpticianAppServiceBase, ISiteAppService
{
    private readonly IRepository<Site, int> _repository;

    /// <summary>
    /// Constructor
    /// </summary>
    public SiteAppService(IRepository<Site, int> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Create a new Site
    /// </summary>
    public async Task CreateAsync(CreateSiteDto input)
    {
        var userId = AbpSession.UserId.Value;
        Site g = new Site();
        var Site = g.Create(input.TenantId, input.Name, input.Address, input.PostalCode, input.Country, input.Phone, input.IsMedicalType, input.Notes, input.InvoiceCurrency, userId);
        var SiteFromDb = await _repository.InsertAsync(Site);
        UnitOfWorkManager.Current.SaveChanges();
    }


    public async Task<List<SiteDto>> GetSiteByTenant(int tenantId)
    {
        var query = from g in _repository.GetAll()
                    where g.TenantId == tenantId
                    select new SiteDto
                    {
                        Id = g.Id,
                        Name = g.Name,
                        Address = g.Address,
                        PostalCode = g.PostalCode,
                        Country = g.Country,
                        Phone = g.Phone,
                        CreationTime = g.CreationTime,
                        TenantId = tenantId,
                        InvoiceCurrency = g.InvoiceCurrency,
                        Notes = g.Notes,
                        CreatorUserId = g.CreatorUserId,
                        IsMedicalType = g.IsMedicalType,

                    };

        var sites = await query.ToListAsync();

        return sites;
    }
    /// <summary>
    /// Get a Site
    /// </summary>
    public async Task<SiteDetailDto> GetAsync(EntityDto input)
    {
        var query = from g in _repository.GetAll()
                    where g.Id == input.Id
                    select new SiteDetailDto
                    {
                        Id = g.Id,
                        Name = g.Name,
                        Address = g.Address,
                        PostalCode = g.PostalCode,
                        Country = g.Country,
                        Phone = g.Phone,
                        CreationTime = g.CreationTime,
                        TenantId = g.TenantId,
                        InvoiceCurrency = g.InvoiceCurrency,
                        Notes = g.Notes,
                        CreatorUserId = g.CreatorUserId,
                        IsMedicalType = g.IsMedicalType,

                    };

        var detail = await query.FirstOrDefaultAsync();
        return detail;

    }



    /// <summary>
    /// Update a Site
    /// </summary>
    public async Task UpdateAsync(UpdateSiteDto input)
    {
        var SiteFromDb = await _repository.GetAsync(input.Id);
        if (SiteFromDb == null)
            throw new UserFriendlyException(OpticianConsts.ErrorMessages.NotFound);
        ObjectMapper.Map(input, SiteFromDb);

    }

    /// <summary>
    /// Delete a Site
    /// </summary>
    public async Task DeleteAsync(EntityDto input)
    {
        var SiteFromDb = await _repository.GetAsync(input.Id);
        if (SiteFromDb == null)
            throw new UserFriendlyException(OpticianConsts.ErrorMessages.NotFound);

        await _repository.DeleteAsync(SiteFromDb);
    }

    /// <summary>
    /// Get all Site
    /// </summary>
    public async Task<ListResultDto<SiteDto>> GetAllAsync()
    {
        var Site = await (
            from g in _repository.GetAll()
            select new SiteDto
            {
                Id = g.Id,
                Name = g.Name,
                Address = g.Address,
                PostalCode = g.PostalCode,
                Country = g.Country,
                Phone = g.Phone,
                CreationTime = g.CreationTime,
                TenantId = g.TenantId,
                InvoiceCurrency = g.InvoiceCurrency,
                Notes = g.Notes,
                CreatorUserId = g.CreatorUserId,
                IsMedicalType = g.IsMedicalType,

            }
        ).ToListAsync();

        return new ListResultDto<SiteDto>(Site);
    }


    #region  GetPagedResult
    /// <summary>
    /// Get Paged Site
    /// </summary>
    public async Task<PagedResultDto<SiteDto>> GetPagedResultAsync(PagedSiteResultRequestDto input)
    {
        var query = _repository.GetAll();
        query = ApplyFilters(input, query);
        IQueryable<SiteDto> selectQuery = GetSelectQuery(query);
        return await selectQuery.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
    }
    private static IQueryable<SiteDto> GetSelectQuery(IQueryable<Site> query)
    {
        return query.Select(g => new SiteDto
        {
            Id = g.Id,
            Name = g.Name,
            Address = g.Address,
            PostalCode = g.PostalCode,
            Country = g.Country,
            Phone = g.Phone,
            CreationTime = g.CreationTime,
            TenantId = g.TenantId,
            InvoiceCurrency = g.InvoiceCurrency,
            Notes = g.Notes,
            CreatorUserId = g.CreatorUserId,
            IsMedicalType = g.IsMedicalType,

        });
    }

    private static IQueryable<Site> ApplyFilters(PagedSiteResultRequestDto input, IQueryable<Site> query)
    {
        if (input.tenantId > 0)
        {
            query = query.Where(g => g.TenantId == input.tenantId);
        }
        if (string.IsNullOrWhiteSpace(input.Keyword) == false)
        {
            query = query.Where(g => g.Name.Contains(input.Keyword));
        }


        return query;
    }

    #endregion


    #region Private Methods


    #endregion
}