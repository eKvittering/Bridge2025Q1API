using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webminux.Optician.Application.InvoiceLines.Dtos;
using Webminux.Optician.Application.Invoices.dtos;
using Webminux.Optician.CommonDtos;
using Webminux.Optician.Core.Invoices;
using Webminux.Optician.Helpers;
using Webminux.Optician.Invoices.dtos;
using static Webminux.Optician.OpticianConsts;

namespace Webminux.Optician.Application.Invoices
{
    /// <summary>
    /// This is a class for <see cref="Invoice"/> Operations entity.
    /// </summary>
    public class InvoiceAppService : OpticianAppServiceBase, IInvoiceAppService
    {

        private readonly IRepository<Invoice> _invoiceRepository;
        private readonly IRepository<Webminux.Optician.ProductItem.ProductItem> _productSerialNoRepository;
        private readonly IRepository<InvoiceLine> _invoiceLineRepository;
        private readonly IRepository<Product> _productRepository;

        /// <summary>  
        /// This is a Constructor for <see cref="InvoiceAppService"/>.
        /// </summary>
        public InvoiceAppService(
            IRepository<Invoice> invoiceRepository,
            IRepository<Webminux.Optician.ProductItem.ProductItem> productSerialNoRepository,
            IRepository<Product> productRepository,
            IRepository<InvoiceLine> invoiceLineRepository)
        {
            _invoiceRepository = invoiceRepository;
            _productSerialNoRepository = productSerialNoRepository;
            _invoiceLineRepository = invoiceLineRepository;
            _productRepository = productRepository;
        }

        #region Create
        /// <summary>
        /// This is a method to create <see cref="Invoice"/> entity.
        /// </summary>
        public async Task CreateAsync(CreateInvoiceDto input)
        {
            var tenantId = AbpSession.TenantId.Value;
            await SetInvoiceNumber(input);
            var invoice = Invoice.Create(tenantId, input.InvoiceNo, input.InvoiceDate, input.DueDate, input.Currency, input.Amount, input.Comment, input.CustomerId);
            AddInvoiceLinesToInvoice(input.InvoiceLines, tenantId, invoice);
            UpdateProductQuantity(input);

            await _invoiceRepository.InsertAsync(invoice);
        }

        private async Task SetInvoiceNumber(CreateInvoiceDto input)
        {
            var invoiceCount = await _invoiceRepository.CountAsync();
            input.InvoiceNo = FormateInoviceNumber(invoiceCount++);
        }

        private string FormateInoviceNumber(int v)
        {
            if (v.ToString().Length == 1)
                return string.Format("0{0}", v);
            return v.ToString();
        }

        private void UpdateProductQuantity(CreateInvoiceDto input)
        {
            var productsNumbers = input.InvoiceLines.GroupBy(p => p.ProductName);
            foreach (var productNo in productsNumbers)
            {
                var product = _productRepository.FirstOrDefault(p => p.ProductNumber == productNo.First().ProductNumber);
                product.InStock = product.InStock - input.InvoiceLines.Count();
            }
        }

        private void AddInvoiceLinesToInvoice(ICollection<InvoiceLineDto> invoiceLines, int tenantId, Invoice invoice)
        {
            invoice.InvoiceLines.Clear();
            foreach (var invoiceLine in invoiceLines)
            {
                var nextInvoiceNumber = _invoiceLineRepository.Count() + 1;

                invoice.InvoiceLines.Add(InvoiceLine.Create(tenantId, nextInvoiceNumber.ToString(), invoiceLine.Amount, invoiceLine.Discount
                    , invoiceLine.CostPrice, null, invoiceLine.Reference, invoiceLine.SerialNumber, invoiceLine.ProductSerialId, invoiceLine.ProductNumber, invoiceLine.ProductName, invoiceLine.Quantity ?? 0, status: InvoiceLineStatuses.AwaitingSerialNumber));
            }
        }


        #endregion
        /// <summary>
        /// This is a method to Update <see cref="Invoice"/> entity.
        /// </summary>
        public async Task UpdateAsync(InvoiceDto input)
        {
            await _invoiceLineRepository.DeleteAsync(invoiceLine => invoiceLine.InvoiceId == input.Id);

            var invoice = await _invoiceRepository.GetAsync(input.Id);
            if (invoice == null)
                throw new EntityNotFoundException(typeof(Invoice), input.Id);

            ObjectMapper.Map(input, invoice);
            AddInvoiceLinesToInvoice(input.InvoiceLines, invoice.TenantId, invoice);
        }

        /// <summary>
        /// This is a method to delete <see cref="Invoice"/> entity.
        /// </summary>
        public async Task DeleteAsync(EntityDto<int> input)
        {
            var invoice = await _invoiceRepository.GetAsync(input.Id);
            if (invoice == null)
                throw new EntityNotFoundException(typeof(Invoice), input.Id);
            await _invoiceRepository.DeleteAsync(invoice);
        }

        /// <summary>
        /// This is a method to get <see cref="Invoice"/> entities.
        /// </summary>
        public async Task<InvoiceDto> GetAsync(EntityDto<int> input)
        {
            var invoice = _invoiceRepository.GetAll();
            if (invoice == null)
                throw new EntityNotFoundException(typeof(Invoice), input.Id);
            var selectQuery = GetDetailSelectQuery(invoice);

            return await selectQuery.FirstOrDefaultAsync(i => i.Id == input.Id);
        }

        /// <summary>
        /// This is a method to get all <see cref="Invoice"/> entities.
        /// </summary>
        public async Task<PagedResultDto<InvoiceDto>> GetPagedResultAsync(PagedInvoiceResultRequestDto input)
        {
            var query = _invoiceRepository.GetAll();

            query = GetFilteredQuery(input, query);
            IQueryable<InvoiceDto> selectQuery = GetSelectQuery(query);

            var result = await selectQuery.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
            return result;
        }

        /// <summary>
        /// Provide Next Invoice Number
        /// </summary>
        /// <returns></returns>
        public async Task<SingleValueDto<string>> GetInvoiceNumber()
        {
            var lineNumber = await _invoiceRepository.CountAsync() + 1;
            var valueToReturn = new SingleValueDto<string>(lineNumber.ToString());
            return valueToReturn;
        }

        public async Task<ListResultDto<LookUpDto<int>>> GetAllAsync()
        {
            var invoices = await _invoiceRepository.GetAll().Where(i => i.IsDraft == false).Select(i => new LookUpDto<int> { Id = i.Id, Name = i.InvoiceNo }).ToListAsync();
            return new ListResultDto<LookUpDto<int>>(invoices);
        }

        #region  Draft Invoice
        /// <summary>
        /// Add Selected Product to draft invoice
        /// </summary>
        /// <param name="input">Product detail and customer id</param>
        public async Task AddItemToDraftInvoice(AddItemToDraftInvoiceDto input)
        {
            var draftInvoice = await _invoiceRepository.FirstOrDefaultAsync(i => i.CustomerId == input.CustomerId && i.IsDraft);
            if (draftInvoice == null)
                draftInvoice = await CreateDraftInvoice(input);

            await AddNewInvoiceLinesToDraftInvoice(input, draftInvoice);
        }

        /// <summary>
        /// Create Draft Invoice from admin site.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task AddItemToDraftInvoiceFromAdmin(AddItemToDraftInvoiceDto input)
        {
            var adminId = AbpSession.UserId ?? 0;
            input.AdminId = adminId;
            var draftInvoice = await _invoiceRepository.FirstOrDefaultAsync(i => i.AdminId == adminId && i.IsDraft);
            if (draftInvoice == null)
                draftInvoice = await CreateDraftInvoiceForAdmin(input);

            await AddNewInvoiceLinesToDraftInvoice(input, draftInvoice);
        }

        /// <summary>
        /// Save Draft Invoice
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SaveDraftInvoice(EntityDto input)
        {
            var draftInvoice = await _invoiceRepository.GetAsync(input.Id);
            draftInvoice.IsDraft = false;

            var invoiceLines = await _invoiceLineRepository.GetAllListAsync(l => l.InvoiceId == draftInvoice.Id);
            foreach (var invoice in invoiceLines)
            {
                invoice.Status = InvoiceLineStatuses.AwaitingSerialNumber;
            }
        }

        /// <summary>
        /// Save Draft Invoice for Admin
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SaveDraftInvoiceForAdmin(SaveDraftInvoiceForAdminDto input)
        {
            var draftInvoice = await _invoiceRepository.GetAsync(input.InvoiceId);
            draftInvoice.IsDraft = false;
            draftInvoice.CustomerId = input.CustomerId;
            draftInvoice.SubCustomerId = input.SubCustomerId;
            var invoiceLines = await _invoiceLineRepository.GetAllListAsync(l => l.InvoiceId == draftInvoice.Id);
            foreach (var invoice in invoiceLines)
            {
                invoice.Status = InvoiceLineStatuses.AwaitingSerialNumber;
            }
        }
        private async Task AddNewInvoiceLinesToDraftInvoice(AddItemToDraftInvoiceDto input, Invoice draftInvoice)
        {
            var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
            var lineCount = await _invoiceLineRepository.CountAsync(i => i.InvoiceId == draftInvoice.Id);

            for (var i = 1; i <= input.Quantity; i++)
            {
                var lineNumber = lineCount + i;
                var invoiceLine = InvoiceLine.Create(tenantId, lineNumber.ToString(), input.Amount, input.Discount,
                    input.CostPrice, draftInvoice.Id, string.Empty, string.Empty, null, quantity: 1, productName: input.ProductName, productNumber: input.ProductNumber);
                await _invoiceLineRepository.InsertAsync(invoiceLine);
            }
        }

        private async Task<Invoice> CreateDraftInvoice(AddItemToDraftInvoiceDto input)
        {
            var invoiceNumber = (await this.GetInvoiceNumber()).Value;

            var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
            var invoice = Invoice.Create(tenantId, invoiceNumber, System.DateTime.Now,
                System.DateTime.Now, "USD", 0, string.Empty, input.CustomerId ?? 0, isDraft: true);
            invoice = await _invoiceRepository.InsertAsync(invoice);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return invoice;
        }

        private async Task<Invoice> CreateDraftInvoiceForAdmin(AddItemToDraftInvoiceDto input)
        {
            var invoiceNumber = (await this.GetInvoiceNumber()).Value;

            var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
            var invoice = Invoice.CreateForAdmin(tenantId, invoiceNumber, System.DateTime.Now,
                System.DateTime.Now, "USD", 0, string.Empty, input.AdminId ?? 0, isDraft: true);
            invoice = await _invoiceRepository.InsertAsync(invoice);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return invoice;
        }
        #endregion

        #region GetCustomerDraftInvoice
        /// <summary>
        /// This is a method to get provided customer draft invoice <see cref="Invoice"/> entities.
        /// </summary>
        public async Task<InvoiceDto> GetCustomerDraftInvoiceAsync(EntityDto<int> input)
        {
            var invoice = _invoiceRepository.GetAll().Where(i => i.CustomerId == input.Id && i.IsDraft);
            var selectQuery = GetDetailSelectQuery(invoice);
            return await selectQuery.FirstOrDefaultAsync();
        }

        /// <summary>
        /// This is a method to get provided customer draft invoice <see cref="Invoice"/> entities.
        /// </summary>
        public async Task<InvoiceDto> GetAdminDraftInvoiceAsync()
        {
            var adminId = AbpSession.UserId ?? 0;
            var invoice = _invoiceRepository.GetAll().Where(i => i.AdminId == adminId && i.IsDraft);
            var selectQuery = GetDetailSelectQuery(invoice);
            return await selectQuery.FirstOrDefaultAsync();
        }
        #endregion
        #region helpers
        private static IQueryable<InvoiceDto> GetSelectQuery(IQueryable<Invoice> query)
        {
            return query.Select(
                            x => new InvoiceDto
                            {
                                Id = x.Id,
                                InvoiceNo = x.InvoiceNo,
                                InvoiceDate = x.InvoiceDate,
                                DueDate = x.DueDate,
                                Currency = x.Currency,
                                Amount = x.Amount,
                                PaidAmount = x.PaidAmount,
                                RemainAmount = x.RemainAmount,
                                Comment = x.Comment,
                                CustomerId = x.CustomerId,
                                CustomerName = x.Customer == null ? string.Empty : x.Customer.User.FullName,
                                SubCustomerName = x.SubCustomer == null ? string.Empty : x.SubCustomer.Name,
                                CreationTime = x.CreationTime,
                                CreatorUserId = x.CreatorUserId,
                                SerialNumber = x.SerialNumber
                            }
                        );
        }

        private static IQueryable<InvoiceDto> GetDetailSelectQuery(IQueryable<Invoice> query)
        {
            return query.Select(
                            x => new InvoiceDto
                            {
                                Id = x.Id,
                                InvoiceNo = x.InvoiceNo,
                                InvoiceDate = x.InvoiceDate,
                                DueDate = x.DueDate,
                                Currency = x.Currency,
                                Amount = x.Amount,
                                PaidAmount = x.PaidAmount,
                                RemainAmount = x.RemainAmount,
                                Comment = x.Comment,
                                CustomerId = x.CustomerId,
                                CustomerUserId = x.Customer == null ? null : x.Customer.UserId,
                                CustomerName = x.Customer == null ? string.Empty : x.Customer.User.FullName,
                                SubCustomerName = x.SubCustomer == null ? string.Empty : x.SubCustomer.Name,
                                CreationTime = x.CreationTime,
                                CreatorUserId = x.CreatorUserId,
                                SerialNumber = x.SerialNumber,
                                InvoiceLines = x.InvoiceLines.Select(invoiceLine => new InvoiceLineDto
                                {
                                    Id = invoiceLine.Id,
                                    LineNo = invoiceLine.LineNo,
                                    InvoiceId = invoiceLine.InvoiceId,
                                    ProductName = invoiceLine.ProductName,
                                    ProductNumber = invoiceLine.ProductNumber,
                                    Amount = invoiceLine.Amount,
                                    CostPrice = invoiceLine.CostPrice,
                                    CreationTime = invoiceLine.CreationTime,
                                    CreatorUserId = invoiceLine.CreatorUserId,
                                    Discount = invoiceLine.Discount,
                                    Quantity = invoiceLine.Quantity,
                                    Reference = invoiceLine.Reference,
                                    TenantId = invoiceLine.TenantId,
                                    SerialNumber = invoiceLine.SerialNumber,
                                    ProductSerialId = invoiceLine.ProductSerialNoId
                                }).ToList()
                            }
                        );
        }

        private static IQueryable<Invoice> GetFilteredQuery(PagedInvoiceResultRequestDto input, IQueryable<Invoice> query)
        {
            if (string.IsNullOrWhiteSpace(input.Keyword) == false)
                query = query.Where(x => x.InvoiceNo.Contains(input.Keyword));
            if (input.CustomerId.HasValue)
                query = query.Where(x => x.CustomerId == input.CustomerId.Value);
            query = query.Where(i => i.IsDraft == false);
            //if (input.CustomerUserId.HasValue)
            //    query = query.Where(x => x.Customer.UserId == input.CustomerUserId.Value);

            return query;
        }
        #endregion
    }
}