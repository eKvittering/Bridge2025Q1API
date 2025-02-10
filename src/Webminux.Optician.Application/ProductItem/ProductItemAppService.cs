using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using Webminux.Optician;
using Webminux.Optician.Activities.Dto;
using Webminux.Optician.Application.ProductItem.Dtos;
using Webminux.Optician.CommonDtos;
using Webminux.Optician.Core.Invoices;
using Webminux.Optician.Helpers;
using Webminux.Optician.ProductItem;
using Webminux.Optician.ProductItem.Dtos;
using Webminux.Optician.Products;
using static Webminux.Optician.OpticianConsts;

/// <summary>
/// Provides methods to manage ProductItems
/// </summary>
[AbpAuthorize()]
public class ProductItemAppService : OpticianAppServiceBase, IProductItemAppService
{
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<ProductItem, int> _repository;
    private readonly IRepository<InvoiceLine, int> _invoiceLineRepository;
    /// <summary>
    /// Constructor
    /// </summary>
    public ProductItemAppService(
        IRepository<Product> productRepository,
        IRepository<InvoiceLine> invoiceLineRepository,
        IRepository<ProductItem, int> repository)

    {
        _productRepository = productRepository;
        _repository = repository;
        _invoiceLineRepository = invoiceLineRepository;
    }



    /// <summary>
    /// Get all ProductItems
    /// </summary>
    public async Task<ListResultDto<ProductItemDto>> GetAllAsync()
    {
        var ProductItems = await (
            from g in _repository.GetAll()
            where g.IsAvailable == false
            select new ProductItemDto
            {
                Id = g.Id,
                Name = g.SerialNumber,
                ProductName = g.Product.Name
            }
        ).ToListAsync();

        return new ListResultDto<ProductItemDto>(ProductItems);
    }

    #region  GetPagedResult
    /// <summary>
    /// Get Paged ProductItems
    /// </summary>
    public async Task<PagedResultDto<ProductItemDto>> GetPagedResultAsync(PagedProductItemResultRequestDto input)
    {
        var query = _repository.GetAll();
        query = ApplyFilters(input, query);
        IQueryable<ProductItemDto> selectQuery = GetSelectQuery(query);
        return await selectQuery.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
    }
    private static IQueryable<ProductItemDto> GetSelectQuery(IQueryable<ProductItem> query)
    {
        return query.Select(g => new ProductItemDto
        {
            Id = g.Id,
            Name = g.Name,
            ProductId = g.ProductId,
            ProductName = g.Product.Name,
            ProductNumber = g.Product.ProductNumber,
            SerialNumber = g.SerialNumber,
            Code = g.Code,
            ReceiverEmployeeDate = g.ReceiverEmployeeDate,
            CreationTime = g.CreationTime,
            CreatorUserId = g.CreatorUserId,
            RecieverEmployeeId = g.RecieverEmployeeId,
            ReceiverEmployeeName = g.ReceiverEmployee.FullName,
            IsAvailable = g.IsAvailable,
            IsMedicalDevice = g.Product.IsMedicalDevice,
            InvoiceId = g.InvoiceId,
            InvoiceNo = g.Invoice.InvoiceNo,
            InvoiceLineId = g.InvoiceLineId,
            InvoiceLineNo = g.InvoiceLine.LineNo,
            ActivityId = g.ActivityId,
            CustomerId = g.Activity != null ? g.Activity.Customer.Customer.Id : null,
            CustomerName = g.Activity != null ? g.Activity.Customer.FullName : string.Empty,
            CustomerEmail = g.Activity != null ? g.Activity.Customer.EmailAddress : string.Empty,
            CustomerUserId = g.Activity != null ? g.Activity.Customer.Id : null,
            SupplierId=g.Product.SupplierId,
            SupplierName=g.Product.Supplier!=null? g.Product.Supplier.User.Name : string.Empty,
            Description = g.Description
        });
    }

    private static IQueryable<ProductItem> ApplyFilters(PagedProductItemResultRequestDto input, IQueryable<ProductItem> query)
    {
        if (string.IsNullOrWhiteSpace(input.Keyword) == false)
            query = query.Where(g => g.SerialNumber.Contains(input.Keyword));
        if (input.IsAvailable.HasValue)
            query = query.Where(g => g.IsAvailable == input.IsAvailable);
        if (input.ProductId.HasValue)
            query = query.Where(g => g.ProductId == input.ProductId);
        if (input.IsMedicalDevice.HasValue)
            query = query.Where(g => g.Product.IsMedicalDevice == input.IsMedicalDevice);
        if (input.ReceiverEmployee.HasValue)
            query = query.Where(g => g.RecieverEmployeeId == input.ReceiverEmployee.Value);
        if (string.IsNullOrWhiteSpace(input.ReciverDate) == false)
        {
            var recriverDate = DateTime.ParseExact(input.ReciverDate, OpticianConsts.DateFormate, System.Globalization.CultureInfo.InvariantCulture);
            query = query.Where(g => g.ReceiverEmployeeDate.Date == recriverDate.Date);
        }

        if (string.IsNullOrWhiteSpace(input.SerialNo) == false)
        {
            query = query.Where(g => g.SerialNumber.Contains(input.SerialNo));
        }

        return query;
    }

    #endregion

    public async Task CreateAsync(CreateProductItemDto input)
    {
        var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
        foreach (var serialNumber in input.SerialNumbers)
        {
            var productItem = ProductItem.Create(tenantId, serialNumber.Value, input.ProductId, AbpSession.UserId.Value, input.ActivityId, input.InvoiceId, input.InvoiceLineId, serialNumber.Code,serialNumber.Description);
            await _repository.InsertAsync(productItem);
            var product = _productRepository.FirstOrDefault(p => p.Id == input.ProductId);
            if (product != null)
            {
                product.InStock++;
                await _productRepository.UpdateAsync(product);
            }
        }
    }

    //public Task UpdateAsync(UpdateProductItemDto input)
    //{
    //    throw new System.NotImplementedException();
    //}

    public async Task<ProductItemDto> GetAsync(EntityDto input)
    {
        var query = _repository.GetAll().Where(p => p.Id == input.Id);
        var selectQuery = GetSelectQuery(query);
        return await selectQuery.FirstOrDefaultAsync();
    }

    public async Task DeleteAsync(EntityDto input)
    {
        var productItem = await _repository.FirstOrDefaultAsync(input.Id);
        await _repository.DeleteAsync(productItem);
    }

    /// <summary>
    /// Get all ProductItems
    /// </summary>
    public async Task<ListResultDto<ProductItemDto>> GetProductItemsOfProduct(EntityDto input)
    {
        var ProductItems = await (
            from g in _repository.GetAll()
            where g.ProductId == input.Id && g.IsSold == false
            select new ProductItemDto
            {
                Id = g.Id,
                Name = g.SerialNumber,
            }
        ).ToListAsync();

        return new ListResultDto<ProductItemDto>(ProductItems);
    }

    /// <summary>
    /// Sale Product Item
    /// </summary>
    /// <returns></returns>
    public async Task UpdateProductItemInformation(int productItemId, int? invoiceId, int? invoiceLineId, int? activityId, string note)
    {
        var productItem = await _repository.GetAsync(productItemId);
        if (invoiceId.HasValue)
        {
            productItem.InvoiceId = invoiceId;
        }
        if (invoiceLineId.HasValue)
        {
            productItem.InvoiceLineId = invoiceLineId;
            var invoiceLine = await _invoiceLineRepository.GetAsync(invoiceLineId ?? 0);
            if (invoiceLine != null)
            {
                invoiceLine.Status = InvoiceLineStatuses.Completed;
                invoiceLine.ProductSerialNoId = productItem.Id;
                invoiceLine.SerialNumber = productItem.SerialNumber;
            }
        }
        productItem.ActivityId = activityId;
        productItem.IsAvailable = false;
        productItem.Note = note;
    }

    /// <summary>
    /// Sale Product Item
    /// </summary>
    /// <param name="productItemId">Product item id and product id</param>
    /// <returns></returns>
    public async Task ReturnSaleProductItem(int productItemId)
    {
        var productItem = await _repository.GetAsync(productItemId);
        productItem.InvoiceId = null;
        productItem.InvoiceLineId = null;
        productItem.IsAvailable = false;
        productItem.ActivityId = null;
    }

    public async Task<ProductItemActivityDto> GetProductItemByActivity(EntityDto input)
    {
        var productItem = await (_repository.GetAll()
            .Where(f => f.ActivityId == input.Id)
            .Select(productItem => new ProductItemActivityDto
            {
                PreviousProductItemId = productItem.Id,
                ProductItemId = productItem.Id,
                ActivityId = productItem.ActivityId,
                InvoiceId = productItem.InvoiceId,
                InvoiceLineId = productItem.InvoiceLineId,
                SerialNumber = productItem.SerialNumber,
                ProductName = productItem.Product.Name,
                Note = productItem.Note
            })).FirstOrDefaultAsync();

        return productItem;
    }

    public async Task UpdateProductItem(ProductItemActivityDto input)
    {
        await ReturnSaleProductItem(input.PreviousProductItemId);
        await UnitOfWorkManager.Current.SaveChangesAsync();
        await UpdateProductItemInformation(input.ProductItemId, input.InvoiceId, input.InvoiceLineId, input.ActivityId, input.Note);
        await UnitOfWorkManager.Current.SaveChangesAsync();
    }

    public async Task<List<ItemDetailDto>> IsAlreadyExist(IsAlreadySerialNumbers input)
    {
        var serialNumbers = input.SerialNumber.Select(s => s.Value).ToList();
        var productItems = await _repository.GetAll().Where(p => serialNumbers.Contains(p.SerialNumber))
            .Select(p => new ItemDetailDto
            {
                SerialNumber = p.SerialNumber,
                Description = p.Description
        
            }).ToListAsync();
        return productItems;
    }

    /// <summary>
    /// Get all ProductItems on base of product number
    /// </summary>
    public async Task<ListResultDto<ProductItemDto>> GetProductItemsWithProductNumber(EntityDto<string> input)
    {
        var ProductItems = await (
            from g in _repository.GetAll()
            where g.Product.ProductNumber == input.Id && g.IsAvailable
            select new ProductItemDto
            {
                Id = g.Id,
                Name = g.SerialNumber,
                ProductName = g.Product.Name
            }
        ).ToListAsync();

        return new ListResultDto<ProductItemDto>(ProductItems);
    }
}