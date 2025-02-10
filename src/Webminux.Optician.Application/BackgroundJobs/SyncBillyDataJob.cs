using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using RestSharp;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Webminux.Optician.Activities;
using Webminux.Optician.Application.Customers.Dtos;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.BackgroundJobs.Dto;
using Webminux.Optician.Companies;
using Webminux.Optician.Core;
using Webminux.Optician.Core.Customers;
using Webminux.Optician.Core.Invoices;

namespace Webminux.Optician.BackgroundJobs
{

    public class SyncBillyDataJob : SyncDataJobBase, ITransientDependency
    {
        public SyncBillyDataJob(IRepository<User, long> userRepository, IRepository<EconomicSyncHistory, int> economicSyncHistoryRepository, IRepository<SyncHistoryDetail, int> syncHistoryDetailRepository, IRepository<Customer, int> customerRepository, IRepository<Invoice, int> invoiceRepository, IRepository<UserType, int> userTypeRepository, IRepository<InvoiceLine, int> invoiceLineRepository, IUnitOfWorkManager unitOfWorkManager, IObjectMapper objectMapper, UserManager userManager, IAbpSession session, IRepository<ActivityArt> activityArtRepository, IRepository<ActivityType> activityTypeRepository, IRepository<Company> companyRepository, IActivityManager activityManager, IRepository<Product, int> productRepository, IRepository<ProductGroup, int> productGroupRepository) : base(userRepository, economicSyncHistoryRepository, syncHistoryDetailRepository, customerRepository, invoiceRepository, userTypeRepository, invoiceLineRepository, unitOfWorkManager, objectMapper, userManager, session, activityArtRepository, activityTypeRepository, companyRepository, activityManager, productRepository, productGroupRepository)
        {
            _BaseUrl = "https://api.billysbilling.com/v2/";
        }

        public override async Task Execute(DataImportJobInputDto args)
        {
            using (_session.Use(args.TenantId, args.UserId))
            {
                Company company;
                EconomicSyncHistory history;
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    history = EconomicSyncHistory.Create(args.TenantId, true);
                    _economicSyncHistoryRepository.Insert(history);
                    userTypeId = _userTypeRepository.GetAll().Where(x => x.Name == OpticianConsts.UserTypes.Customer).FirstOrDefault().Id;
                    company = await _companyRepository.FirstOrDefaultAsync(company => company.TenantId == args.TenantId);
                    unitOfWork.Complete();
                }
                await SyncCustomers(args, company, history);
                await SyncProducts(args, company, history, "");
                await SyncInvoices(args, company, history, "");

                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    history.IsInProcess = false;
                    _economicSyncHistoryRepository.Update(history);
                    unitOfWork.Complete();
                }
            }
        }
        private async Task SyncInvoices(DataImportJobInputDto args, Company company, EconomicSyncHistory history, string lastInvoiceNo)
        {
            var pageIndex = 1;
            RestResponse response = await GetBillyInvoices(pageIndex, company, lastInvoiceNo);

            if (response.IsSuccessful)
            {
                pageIndex++;

                var result = JsonSerializer.Deserialize<BillyInvoiceApiResponseDto>(response.Content);

                var billyInvoices = result.invoices;
                var totalRecord = result.meta.paging.total;

                while (totalRecord > (pageIndex * 1000))
                {
                    response = await GetBillyInvoices(pageIndex, company, lastInvoiceNo);
                    result = JsonSerializer.Deserialize<BillyInvoiceApiResponseDto>(response.Content);
                    billyInvoices.AddRange(result.invoices);
                    pageIndex++;
                }

                var index = 0;
                foreach (var billyInvoice in billyInvoices)
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        var customer = _customerRepository.GetAll().Where(x => x.CustomerNo == billyInvoice.contactId && x.IsSync == true).Select(customer => new
                        {
                            customer.Id,
                            CustomerUserId = customer.UserId
                        }).FirstOrDefault();

                        if (customer != null)
                        {
                            var invoiceDbo = _invoiceRepository.GetAll().Where(x => x.InvoiceNo == billyInvoice.id).FirstOrDefault();
                            if (invoiceDbo == null)
                            {
                                var invoice = MapBillyInvoiceToInvoice(billyInvoice, args.TenantId, args.UserId, customer.Id);
                                invoiceDbo = invoice;
                            }
                            else
                            {
                                invoiceDbo.InvoiceNo = billyInvoice.id;
                                invoiceDbo.InvoiceDate = billyInvoice.createdTime;
                                invoiceDbo.DueDate = DateTime.Parse(billyInvoice.dueDate);
                                invoiceDbo.Currency = billyInvoice.currencyId ?? "";
                                invoiceDbo.Amount = Convert.ToDecimal(billyInvoice.grossAmount);
                                invoiceDbo.CustomerId = customer.Id;
                            }
                            _invoiceRepository.InsertOrUpdate(invoiceDbo);
                            _unitOfWorkManager.Current.SaveChanges();

                            foreach (var line in billyInvoice.lines)
                            {
                                SyncBillyInvoiceLine(args, invoiceDbo, line);
                            }
                            _unitOfWorkManager.Current.SaveChanges();
                            await CreateSaleActivityWhileSyncInvoice(customer.CustomerUserId, invoiceDbo.InvoiceNo.ToString(), args.TenantId);
                        }
                        unitOfWork.Complete();
                    }
                    index++;
                };

            }
            else
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    history.IsFailed = true;
                    _economicSyncHistoryRepository.Update(history);
                    unitOfWork.Complete();
                }
            }
        }
        private async Task SyncProducts(DataImportJobInputDto args, Company company, EconomicSyncHistory history, string lastInvoiceNo)
        {
            var pageIndex = 1;
            RestResponse response = await GetBillyProducts(pageIndex, company);

            if (response.IsSuccessful)
            {
                pageIndex++;

                var result = JsonSerializer.Deserialize<BillyProductApiResponseDto>(response.Content);

                var billyProducts = result.products;
                var totalRecord = result.meta.paging.total;

                while (totalRecord > (pageIndex * 1000))
                {
                    response = await GetBillyProducts(pageIndex, company);
                    result = JsonSerializer.Deserialize<BillyProductApiResponseDto>(response.Content);
                    billyProducts.AddRange(result.products);
                    pageIndex++;
                }

                var index = 0;
                foreach (var billyProduct in billyProducts)
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        var productDbo = _productRepository.GetAll().Where(x => x.ProductNumber == billyProduct.id).FirstOrDefault();
                        if (productDbo == null)
                        {
                            var product = MapBillyProductToProduct(billyProduct, args.TenantId);
                            productDbo = product;
                        }
                        else
                        {
                            productDbo.Name = billyProduct.name;
                            productDbo.Description = billyProduct.description;
                            productDbo.SalesPrice = billyProduct.prices == null ? 0 : (double)billyProduct.prices.FirstOrDefault()?.unitPrice;
                        }
                        _productRepository.InsertOrUpdate(productDbo);
                        unitOfWork.Complete();
                    }
                    index++;
                };

            }
            else
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    history.IsFailed = true;
                    _economicSyncHistoryRepository.Update(history);
                    unitOfWork.Complete();
                }
            }
        }

        private void SyncBillyInvoiceLine(DataImportJobInputDto args, Invoice invoiceDbo, BillyInvoiceLineDto line)
        {
            var invoiceLineDbo = _invoiceLineRepository.GetAll().Where(x => x.LineNo == line.id).FirstOrDefault();
            if (invoiceLineDbo == null)
            {
                var invoiceLine = InvoiceLine.Create(
                    args.TenantId,
                    line.id,
                    line.amount ?? 0,
                    0, // discount value
                    line.unitPrice ?? 0,
                    invoiceDbo.Id,
                    line.id,
                    string.Empty, null,
                    line.product.id,
                    line.product.name,
                    line.quantity
                    );
                invoiceLineDbo = invoiceLine;
            }
            else
            {
                invoiceLineDbo.Quantity = line.quantity;
                invoiceLineDbo.ProductNumber = line.product.id;
                invoiceLineDbo.ProductName = line.product.name;
                invoiceLineDbo.Amount = line.amount ?? 0;
            }
            _invoiceLineRepository.InsertOrUpdate(invoiceLineDbo);
            _unitOfWorkManager.Current.SaveChanges();
        }

        private Invoice MapBillyInvoiceToInvoice(BillyInvoiceDto billyInvoice, int tenantId, long userId, int customerId)
        {
            var invoice = Invoice.Create(
                    tenantId,
                    billyInvoice.id,
                    billyInvoice.createdTime,
                    string.IsNullOrWhiteSpace(billyInvoice.dueDate) ? DateTime.UtcNow : DateTime.Parse(billyInvoice.dueDate),
                    billyInvoice.currencyId ?? "",
                    Convert.ToDecimal(billyInvoice.grossAmount),
                    billyInvoice.lineDescription,
                    customerId,
                    true
                );
            return invoice;
        }
        private Product MapBillyProductToProduct(BillyProductDto billyProduct, int tenantId)
        {
            var product = Product.Create(
                false,
                    tenantId,
                    billyProduct.id,
                    billyProduct.name,
                    billyProduct.description,
                    0,
                    billyProduct.prices == null ? 0 : (double)billyProduct.prices.FirstOrDefault()?.unitPrice,
                    0,
                    0,
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    0,
                    0,
                    0,
                    0,
                    null,
                    null,
                    0,
                    0,
                    null,
                    null
                );
            return product;
        }
        private async Task SyncCustomers(DataImportJobInputDto args, Company company, EconomicSyncHistory history)
        {
            var pageIndex = 1;
            RestResponse response = await GetBillyCustomers(pageIndex, company);

            if (response.IsSuccessful)
            {
                pageIndex++;
                var result = JsonSerializer.Deserialize<BillyContactApiResponseDto>(response.Content);
                var billyCustomers = result.contacts;
                var totalRecord = result.meta.paging.total;

                while (totalRecord > (pageIndex * _defaulPageSize))
                {
                    response = await GetBillyCustomers(pageIndex, company);
                    result = JsonSerializer.Deserialize<BillyContactApiResponseDto>(response.Content);
                    billyCustomers.AddRange(result.contacts);
                    pageIndex++;
                }
                var index = 0;
                foreach (var billyCustomer in billyCustomers)
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        var customerDbo = _customerRepository.GetAll().Where(x => x.TenantId == args.TenantId && x.CustomerNo == billyCustomer.id && x.IsSync == true).FirstOrDefault();
                        var input = _objectMapper.Map<CreateCustomerDto>(billyCustomer);
                        input.Password = "Dummy@123";
                        var contactPerson = billyCustomer.contactPersons.LastOrDefault();
                        if (contactPerson is not null)
                            input.EmailAddress = contactPerson.email ?? null;

                        input.EmailAddress = !string.IsNullOrWhiteSpace(input.EmailAddress) ? input.EmailAddress : (input.CustomerNo + "@billy.com");
                        input.UserName = input.EmailAddress;
                        input.Address = billyCustomer.street;
                        input.TownCity = billyCustomer.cityText;
                        input.Postcode = billyCustomer.zipcodeText;
                        input.TelephoneFax = billyCustomer.phone;
                        input.Country = billyCustomer.countryId;

                        User newUser = new User();
                        if (customerDbo == null)
                        {
                            var userDbo = _userManager.Users.Where(x => x.TenantId == args.TenantId && x.EmailAddress == input.EmailAddress).FirstOrDefault();
                            if (userDbo == null)
                            {
                                newUser = _objectMapper.Map<User>(input);
                                newUser.Name = newUser.Name.Truncate(20);
                                newUser.Surname = newUser.Surname?.Truncate(20) ?? "";
                                newUser.TenantId = args.TenantId;
                                newUser.UserTypeId = userTypeId;
                                newUser.IsActive = true;
                                await _userManager.CreateAsync(newUser, input.Password);
                                await _userManager.AddToRoleAsync(newUser, "ADMIN");
                                _unitOfWorkManager.Current.SaveChanges();
                                newUser = _userManager.Users.OrderByDescending(x => x.Id).FirstOrDefault();
                            }
                            else
                            {
                                newUser = userDbo;
                            }
                            if (_customerRepository.FirstOrDefault(x => x.UserId == newUser.Id) == null)
                            {
                                var customer = Customer.Create(args.TenantId,
                                                               input.CustomerNo,
                                                               input.Address ?? "",
                                                               input.Postcode ?? "",
                                                               input.TownCity ?? "",
                                                               input.Country ?? "",
                                                               input.TelephoneFax ?? "",
                                                               input.Website ?? "",
                                                               input.Currency ?? "",
                                                               newUser.Id,
                                                               newUser.Id,
                                                               1,
                                                               null, false, string.Empty);
                                _customerRepository.Insert(customer);
                            }
                        }
                        unitOfWork.Complete();
                    }
                    index++;
                };
            }
            else
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    history.IsFailed = true;
                    _economicSyncHistoryRepository.Update(history);
                    unitOfWork.Complete();
                }
            }
        }

        private async Task<RestResponse> GetBillyInvoices(int pageIndex, Company company, string lastNumber = "")
        {
            var url = _BaseUrl + "invoices?include=invoice.lines:embed,invoiceLine.product:embed&page=" + pageIndex + "&pagesize=" + _defaulPageSize;
            if (!string.IsNullOrWhiteSpace(lastNumber))
            {
                url += "&filter=bookedInvoiceNumber$gt:";
                url += lastNumber;
            }
            var client = new RestClient(url);

            var request = new RestRequest();
            request.AddHeader("X-Access-Token", company.BillyAccessToken);
            var response = await client.ExecuteAsync(request);
            return response;
        }
        private async Task<RestResponse> GetBillyCustomers(int pageIndex, Company company)
        {
            var url = _BaseUrl + "contacts?include=contacts.contactPersons:embed&page=" + pageIndex + "&pagesize=" + _defaulPageSize;
            var client = new RestClient(url);
            var request = new RestRequest();
            request.AddHeader("X-Access-Token", company.BillyAccessToken);
            var response = await client.ExecuteAsync(request);
            return response;
        }
        private async Task<RestResponse> GetBillyProducts(int pageIndex, Company company)
        {
            var url = _BaseUrl + "products?page=" + pageIndex + "&pagesize=" + _defaulPageSize;
            var client = new RestClient(url);
            var request = new RestRequest();
            request.AddHeader("X-Access-Token", company.BillyAccessToken);
            var response = await client.ExecuteAsync(request);
            return response;
        }
    }
}
