using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Webminux.Optician.Application.InvoiceLines.Dtos;
using Webminux.Optician.Core.Invoices;
using Webminux.Optician.Helpers;
using Webminux.Optician.InvoiceLines.Dtos;

namespace Webminux.Optician.Application.InvoiceLines
{
    /// <summary>
    /// This is a class for <see cref="InvoiceLine"/> entity Operations.
    /// </summary>
    public class InvoiceLineAppService : OpticianAppServiceBase, IInvoiceLineAppService
    {
        private readonly IRepository<InvoiceLine> _invoiceLineRepository;
        private readonly IRepository<Optician.ProductItem.ProductItem> _productItemRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceLineAppService"/> class.
        /// </summary>
        public InvoiceLineAppService(
            IRepository<InvoiceLine> invoiceLineRepository,
            IRepository<Webminux.Optician.ProductItem.ProductItem> productItemRepository
            )
        {
            _invoiceLineRepository = invoiceLineRepository;
            _productItemRepository = productItemRepository;
        }

        /// <summary>
        /// This method is used to create InvoiceLines.
        /// </summary>
        public async Task CreateAsync(CreateInvoiceLineDto input)
        {
            var tenantId = AbpSession.TenantId.Value;

            var invoiceLine = InvoiceLine.Create(tenantId, input.LineNo, input.Amount, input.Discount, input.CostPrice, input.InvoiceId, input.Reference, string.Empty, null);
            await _invoiceLineRepository.InsertAsync(invoiceLine);
        }

        /// <summary>
        /// This method is used to update InvoiceLines.
        /// </summary>
        public async Task UpdateAsync(InvoiceLineDto input)
        {
            var entity = await _invoiceLineRepository.GetAsync(input.Id);
            if (entity == null)
                throw new EntityNotFoundException(typeof(InvoiceLine), input.Id);

            ObjectMapper.Map(input, entity);
        }

        /// <summary>
        /// This method is used to delete InvoiceLines.
        /// </summary>
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var entity = await _invoiceLineRepository.GetAsync(input.Id);
            if (entity == null)
                throw new EntityNotFoundException(typeof(InvoiceLine), input.Id);

            await _invoiceLineRepository.DeleteAsync(entity);
        }

        /// <summary>
        /// This method is used to get InvoiceLines.
        /// </summary>
        public async Task<InvoiceLineDto> GetAsync(EntityDto<int> input)
        {
            var entity = await _invoiceLineRepository.GetAsync(input.Id);
            if (entity == null)
                throw new EntityNotFoundException(typeof(InvoiceLine), input.Id);

            return ObjectMapper.Map<InvoiceLineDto>(entity);
        }

        /// <summary>
        /// This method is used to get all InvoiceLines.
        /// </summary>
        public async Task<PagedResultDto<InvoiceLineDto>> GetAllAsync(GetPagedInvoiceLinesResultRequestDto input)
        {
            var query = _invoiceLineRepository.GetAll();
            query = GetAllApplyFilters(input, query);
            IQueryable<InvoiceLineDto> invoiceLinesSelectQuery = GetInvoiceLineSelectQuery(query);
            return await invoiceLinesSelectQuery.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
        }

        /// <summary>
        /// Get All invoice lines of provided invoice
        /// </summary>
        /// <param name="input">Invoice id</param>
        /// <returns>List of all invoice lines against invoice</returns>
        public async Task<ListResultDto<InvoiceLineDto>> GetInvoiceLinesByInvoice(EntityDto input)
        {
            var query = _invoiceLineRepository.GetAll().Where(invoiceLine => invoiceLine.InvoiceId == input.Id);
            IQueryable<InvoiceLineDto> invoiceLinesSelectQuery = GetInvoiceLineSelectQuery(query);
            var invoiceLines = await invoiceLinesSelectQuery.ToListAsync();
            return new ListResultDto<InvoiceLineDto>(invoiceLines);
        }

        /// <summary>
        /// Add Provided serial Number to invoice line
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task AddSerialNumber(AddSerialNumberDto input)
        {
            var invoiceLine = await _invoiceLineRepository.GetAsync(input.Id);
            invoiceLine.ProductSerialNoId = input.ProductItemId;
            invoiceLine.SerialNumber = input.SerialNumber;

            var productItem = await _productItemRepository.GetAsync(input.ProductItemId);
            productItem.InvoiceId = invoiceLine.InvoiceId;
            productItem.InvoiceLineId = invoiceLine.Id;
            productItem.IsAvailable = false;
        }

        private static IQueryable<InvoiceLineDto> GetInvoiceLineSelectQuery(IQueryable<InvoiceLine> query)
        {
            return query.Select(i => new InvoiceLineDto
            {
                Id = i.Id,
                Amount = i.Amount,
                CostPrice = i.CostPrice,
                Discount = i.Discount,
                InvoiceId = i.InvoiceId,
                LineNo = i.LineNo,
                Reference = i.Reference,
                TenantId = i.TenantId,
                CreationTime = i.CreationTime,
                CreatorUserId = i.CreatorUserId,
                ProductName = i.ProductName,
                ProductNumber = i.ProductNumber,
                Quantity = i.Quantity,
                SerialNumber = i.SerialNumber,
                ProductSerialId = i.ProductSerialNoId,
                Status=i.Status,
                SupplierId=i.ProductSerialNo!=null?i.ProductSerialNo.Product.SupplierId.HasValue?i.ProductSerialNo.Product.SupplierId:null:null,
                SupplierName=i.ProductSerialNoId.HasValue?i.ProductSerialNo.Product.SupplierId.HasValue?i.ProductSerialNo.Product.Supplier.User.Name :string.Empty:string.Empty
            });
        }

        private static IQueryable<InvoiceLine> GetAllApplyFilters(GetPagedInvoiceLinesResultRequestDto input, IQueryable<InvoiceLine> query)
        {
            if (input.InvoiceId.HasValue)
                query = query.Where(invoiceLine => invoiceLine.InvoiceId == input.InvoiceId);
            return query;
        }
    }
}