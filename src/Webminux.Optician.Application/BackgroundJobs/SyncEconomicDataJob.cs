using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using Castle.Core.Resource;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Webminux.Optician.Activities;
using Webminux.Optician.Application.Activities.Dto;
using Webminux.Optician.Application.Customers.Dtos;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.BackgroundJobs;
using Webminux.Optician.Companies;
using Webminux.Optician.Core;
using Webminux.Optician.Core.Customers;
using Webminux.Optician.Core.Invoices;
using Webminux.Optician.Helpers;
using Webminux.Optician.ProductGroups.Dto;
using Webminux.Optician.Products.Dtos;

namespace Webminux.Optician
{
    public class SyncEconomicDataJob : SyncDataJobBase
    {
        public SyncEconomicDataJob(IRepository<User, long> userRepository, IRepository<EconomicSyncHistory, int> economicSyncHistoryRepository, IRepository<SyncHistoryDetail, int> syncHistoryDetailRepository, IRepository<Customer, int> customerRepository, IRepository<Invoice, int> invoiceRepository, IRepository<UserType, int> userTypeRepository, IRepository<InvoiceLine, int> invoiceLineRepository, IUnitOfWorkManager unitOfWorkManager, IObjectMapper objectMapper, UserManager userManager, IAbpSession session, IRepository<ActivityArt> activityArtRepository, IRepository<ActivityType> activityTypeRepository, IRepository<Company> companyRepository, IActivityManager activityManager, IRepository<Product, int> productRepository, IRepository<ProductGroup, int> productGroupRepository) : base(userRepository, economicSyncHistoryRepository, syncHistoryDetailRepository, customerRepository, invoiceRepository, userTypeRepository, invoiceLineRepository, unitOfWorkManager, objectMapper, userManager, session, activityArtRepository, activityTypeRepository, companyRepository, activityManager, productRepository, productGroupRepository)
        {
            _BaseUrl = "https://restapi.e-conomic.com/";
        }

        public override async Task Execute(DataImportJobInputDto args)
        {
            try
            {
                using (_session.Use(args.TenantId, args.UserId))
                {
                    Company company;
                    var lastInvoiceNo = "";
                    var lastCustomerNo = "";
                    var lastProductNo = "";
                    var lastProductGroupNo = "";
                    EconomicSyncHistory history;
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        history = EconomicSyncHistory.Create(args.TenantId, true);
                        _economicSyncHistoryRepository.Insert(history);
                        userTypeId = _userTypeRepository.GetAll().Where(x => x.Name == OpticianConsts.UserTypes.Customer).FirstOrDefault().Id;
                        company = await _companyRepository.FirstOrDefaultAsync(company => company.TenantId == args.TenantId);

                        var lastCustomer = _customerRepository.GetAll().Where(x => x.TenantId == args.TenantId).OrderByDescending(x => x.Id).FirstOrDefault();
                        lastCustomerNo = lastCustomer != null ? lastCustomer.CustomerNo : "";

                        var lastInvoice = _invoiceRepository.GetAll().Where(x => x.TenantId == args.TenantId).OrderByDescending(x => x.Id).FirstOrDefault();
                        lastInvoiceNo = lastInvoice != null ? lastInvoice.InvoiceNo : "";

                        var lastProduct = _productRepository.GetAll().Where(x => x.TenantId == args.TenantId).OrderByDescending(x => x.Id).FirstOrDefault();
                        lastProductNo = lastProduct != null ? lastProduct.ProductNumber : "";

                        var lastProductGroup = _productGroupRepository.GetAll().Where(x => x.TenantId == args.TenantId).OrderByDescending(x => x.Id).FirstOrDefault();
                        lastProductGroupNo = lastProductGroup != null ? lastProductGroup.ProductGroupNumber.ToString() : "";

                        unitOfWork.Complete();


                    }

                    await SyncCustomers(args, company, history, lastCustomerNo);
                    await SyncProductGroups(args, company, lastProductGroupNo,history);
                    await SyncProducts(args, company, lastProductNo,lastProductGroupNo, history);
                    await SyncInvoices(args, company, history, lastInvoiceNo);

                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        history.IsInProcess = false;
                        _economicSyncHistoryRepository.Update(history);
                        unitOfWork.Complete();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private async Task SyncInvoiceLines(string invoiceNumber, DataImportJobInputDto args, Company company)
        {
            RestResponse response = await GetEconomicInvoicelines(invoiceNumber, company);

            if (response.IsSuccessful)
            {
                var result = JsonSerializer.Deserialize<EconomicInvoiceDto>(response.Content);
                var lines = result.lines;
                foreach (var line in lines)
                {
                    if (line.unit != null && line.product != null)
                    {

                        var invoiceId = _invoiceRepository.GetAll().Where(x => x.InvoiceNo == invoiceNumber).FirstOrDefault().Id;
                        var lineDbo = _invoiceLineRepository.GetAll().Where(x => x.LineNo == line.lineNumber.ToString() && x.InvoiceId == invoiceId).FirstOrDefault();
                        var productDbo = _productRepository.GetAll().Where(x => x.ProductNumber == line.product.productNumber).FirstOrDefault();
                        if (productDbo == null)
                        {
                            RestResponse productResponse = await GetEconomicProduct(line.product.productNumber, company);
                            if (!productResponse.IsSuccessful)
                            {
                                continue;
                            }
                            var productResult = JsonSerializer.Deserialize<EconomicProductDto>(productResponse.Content);
                            productDbo = new Product() { Name = productResult.name };
                        }
                        if (lineDbo == null)
                        {
                            var newline = InvoiceLine.Create(
                                    args.TenantId,
                                    line.lineNumber.ToString(),
                                    Convert.ToDecimal(line.totalNetAmount),
                                    Convert.ToDecimal(line.discountPercentage * line.unitNetPrice),
                                    Convert.ToDecimal(line.unitCostPrice),
                                    invoiceId,
                                    line.lineNumber.ToString(),
                                    string.Empty,
                                    null,
                                    line.product.productNumber,
                                    productDbo.Name,
                                    Convert.ToDouble(line.quantity)
                                    );
                            lineDbo = newline;
                        }
                        else
                        {
                            lineDbo.LineNo = line.lineNumber.ToString();
                            lineDbo.Amount = Convert.ToDecimal(line.totalNetAmount);
                            lineDbo.Discount = Convert.ToDecimal(line.discountPercentage * line.unitNetPrice);
                            lineDbo.CostPrice = Convert.ToDecimal(line.unitCostPrice);
                            lineDbo.InvoiceId = invoiceId;
                            lineDbo.LineNo = line.lineNumber.ToString();
                            lineDbo.ProductNumber = line.product.productNumber;
                            lineDbo.ProductName = productDbo.Name;
                            lineDbo.Quantity = Convert.ToDouble(line.quantity);
                        }
                        _invoiceLineRepository.InsertOrUpdate(lineDbo);
                    }
                };
            }
        }
        private async Task SyncProducts(DataImportJobInputDto args, Company company, string lastProductNo, string lastProductGroupNo, EconomicSyncHistory history)
        {
            var pageIndex = 0;

            RestResponse response = await GetEconomicProducts(pageIndex, company,lastProductNo);

            if (response.IsSuccessful)
            {
                pageIndex++;
                var result = JsonSerializer.Deserialize<EconomicApiResponseDto<EconomicProductDto>>(response.Content);
                var economicProducts = result.collection;
                var totalRecord = result.pagination.results;

                while (totalRecord > (pageIndex * 1000))
                {
                    response = await GetEconomicProductGroups(pageIndex, company, lastProductGroupNo);
                    result = JsonSerializer.Deserialize<EconomicApiResponseDto<EconomicProductDto>>(response.Content);
                    economicProducts.AddRange(result.collection);
                    pageIndex++;
                }

                var products = MapEconomicProductsToProducts(economicProducts, args.TenantId, args.UserId);
                foreach (var product in products)
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        var productDbo = _productRepository.GetAll().Where(x => x.ProductNumber == product.ProductNumber).FirstOrDefault();
                        if (productDbo != null)
                        {
                            productDbo.Name = product.Name;
                            productDbo.ProductGroupNumber = product.ProductGroupNumber;
                            productDbo.SalesPrice = product.SalesPrice;
                            productDbo.Recprice = product.Recprice;
                            productDbo.ProductNumber = product.ProductNumber;
                        }
                        else
                        {
                            productDbo = product;
                        }

                        await _productRepository.InsertOrUpdateAsync(productDbo);
                        await _unitOfWorkManager.Current.SaveChangesAsync();

                        var historyDetail = SyncHistoryDetail.Create(args.TenantId, "Product", productDbo.Id.ToString(), productDbo.Name,history.Id);
                        _syncHistoryDetailRepository.Insert(historyDetail);

                        unitOfWork.Complete();
                    }
                };
            }
        }

        private async Task SyncProductGroups(DataImportJobInputDto args, Company company, string lastProductGroupNo, EconomicSyncHistory history)
        {
            var pageIndex = 0;

            RestResponse response = await GetEconomicProductGroups(pageIndex, company, lastProductGroupNo);

            if (response.IsSuccessful)
            {
                pageIndex++;
                var result = JsonSerializer.Deserialize<EconomicApiResponseDto<EconomicProductGroupDto>>(response.Content);
                var economicProductGroups = result.collection;
                var totalRecord = result.pagination.results;

                while (totalRecord > (pageIndex * 1000))
                {
                    response = await GetEconomicProductGroups(pageIndex, company, lastProductGroupNo);
                    result = JsonSerializer.Deserialize<EconomicApiResponseDto<EconomicProductGroupDto>>(response.Content);
                    economicProductGroups.AddRange(result.collection);
                    pageIndex++;
                }

                var productGroups = MapEconomicProductGroupsToProductGroups(economicProductGroups, args.TenantId, args.UserId);
                foreach (var productGroup in productGroups)
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        var productGroupDbo = _productGroupRepository.GetAll().Where(x => x.ProductGroupNumber == productGroup.ProductGroupNumber).FirstOrDefault();
                        if (productGroupDbo != null)
                        {
                            productGroupDbo.Name = productGroup.Name;
                            productGroupDbo.ProductGroupNumber = productGroup.ProductGroupNumber;
                        }
                        else
                        {
                            productGroupDbo = productGroup;
                        }
                        await _productGroupRepository.InsertOrUpdateAsync(productGroupDbo);
                        await _unitOfWorkManager.Current.SaveChangesAsync();

                        var historyDetail = SyncHistoryDetail.Create(args.TenantId, "ProductGroup", productGroupDbo.Id.ToString(), productGroupDbo.Name,history.Id);
                        _syncHistoryDetailRepository.Insert(historyDetail);

                        unitOfWork.Complete();
                    }
                };
            }
        }

        private async Task SyncInvoices(DataImportJobInputDto args, Company company, EconomicSyncHistory history, string lastInvoiceNo)
        {
            var pageIndex = 0;
            RestResponse response = await GetEconomicInvoices(pageIndex, company, lastInvoiceNo);

            if (response.IsSuccessful)
            {
                pageIndex++;
                var result = JsonSerializer.Deserialize<EconomicApiResponseDto<EconomicInvoiceDto>>(response.Content);
                var economicInvoices = result.collection;
                var totalRecord = result.pagination.results;

                while (totalRecord > (pageIndex * 1000))
                {
                    response = await GetEconomicInvoices(pageIndex, company, lastInvoiceNo);
                    result = JsonSerializer.Deserialize<EconomicApiResponseDto<EconomicInvoiceDto>>(response.Content);
                    economicInvoices.AddRange(result.collection);
                    pageIndex++;
                }

                var index = 0;
                foreach (var econmicInvoice in economicInvoices)
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        var customer = _customerRepository.GetAll().Where(x => x.CustomerNo == econmicInvoice.customer.customerNumber.ToString()).Select(customer => new
                        {
                            customer.Id,
                            CustomerUserId = customer.UserId
                        }).FirstOrDefault();

                        if (customer != null)
                        {
                            var invoice = MapEconomicInvoiceToInvoice(econmicInvoice, args.TenantId, args.UserId, customer.Id);
                            var invoiceDbo = _invoiceRepository.GetAll().Where(x => x.InvoiceNo == invoice.InvoiceNo).FirstOrDefault();
                            if (invoiceDbo == null)
                            {
                                invoiceDbo = invoice;
                            }
                            else
                            {
                                invoiceDbo.InvoiceNo = econmicInvoice.bookedInvoiceNumber.ToString();
                                invoiceDbo.InvoiceDate = econmicInvoice.date;
                                invoiceDbo.DueDate = econmicInvoice.dueDate;
                                invoiceDbo.Currency = econmicInvoice.currency ?? "";
                                invoiceDbo.Amount = Convert.ToDecimal(econmicInvoice.netAmount);
                                invoiceDbo.CustomerId = customer.Id;
                            }
                            _invoiceRepository.InsertOrUpdate(invoiceDbo);
                            _unitOfWorkManager.Current.SaveChanges();

                            var historyDetail = SyncHistoryDetail.Create(args.TenantId, "Invoice", invoiceDbo.Id.ToString(), "Invoice-"+invoiceDbo.InvoiceNo,history.Id);
                            _syncHistoryDetailRepository.Insert(historyDetail);

                            await SyncInvoiceLines(invoice.InvoiceNo, args, company);
                            _unitOfWorkManager.Current.SaveChanges();
                            await CreateSaleActivityWhileSyncInvoice(customer.CustomerUserId, invoice.InvoiceNo.ToString(), args.TenantId);
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

        private async Task CreateSaleActivityWhileSyncInvoice(long customerId, string invoiceNo, int tenantId)
        {
            CreateActivityDto createActivityDto = new CreateActivityDto();
            var activityTypes = new ListResultDto<LookUpDto<int>>();
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                activityTypes = await _activityTypeRepository.GetAll().GetLookupResultAsync<ActivityType, int>();

            }
            var activityArtType = await _activityArtRepository.GetAll().GetLookupResultAsync<ActivityArt, int>();

            var userTypeId = _userTypeRepository.GetAll().Where(x => x.Name == OpticianConsts.UserTypes.Employee).FirstOrDefault().Id;

            createActivityDto.CustomerId = customerId;
            createActivityDto.Name = "Invoice " + "" + invoiceNo;

            createActivityDto.EmployeeId = _userRepository.GetAll().Where(x => x.UserTypeId == userTypeId && x.TenantId == tenantId).FirstOrDefault().Id;
            createActivityDto.ActivityArtId = activityArtType.Items.Where(x => x.Name == OpticianConsts.ActivityArts.Activity).FirstOrDefault().Id;
            createActivityDto.ActivityTypeId = activityTypes.Items.Where(x => x.Name == OpticianConsts.ActivityTypes.Sale).FirstOrDefault().Id;
            createActivityDto.FollowUpTypeId = activityTypes.Items.Where(x => x.Name == OpticianConsts.ActivityTypes.Sale).FirstOrDefault().Id;
            createActivityDto.Date = DateTime.UtcNow.ToString("yyyy-MM-dd");
            createActivityDto.FollowUpDate = DateTime.UtcNow.ToString("yyyy-MM-dd");

            await _activityManager.CreateAsync(GetActivityModel(createActivityDto, tenantId));
        }

        //private async Task SyncCustomers(DataImportJobInputDto args, Company company, EconomicSyncHistory history, string lastCustomerNo)
        //{
        //    var pageIndex = 0;
        //    RestResponse response = await GetEconomicCustomers(pageIndex, company, lastCustomerNo);

        //    if (response.IsSuccessful)
        //    {
        //        pageIndex++;
        //        var result = JsonSerializer.Deserialize<EconomicApiResponseDto<EconomicCustomerDto>>(response.Content);
        //        var economicCustomers = result.collection;
        //        var totalRecord = result.pagination.results;

        //        while (totalRecord > (pageIndex * _defaulPageSize))
        //        {
        //            response = await GetEconomicCustomers(pageIndex, company, lastCustomerNo);
        //            result = JsonSerializer.Deserialize<EconomicApiResponseDto<EconomicCustomerDto>>(response.Content);
        //            economicCustomers.AddRange(result.collection);
        //            pageIndex++;
        //        }
        //        var index = 0;
        //        foreach (var economicCustomer in economicCustomers)
        //        {
        //            using (var unitOfWork = _unitOfWorkManager.Begin())
        //            {
        //                var customerDbo = _customerRepository.GetAll().Where(x => x.TenantId == args.TenantId
        //                && x.CustomerNo == economicCustomer.customerNumber.ToString() ).FirstOrDefault();

        //                var input = _objectMapper.Map<CreateCustomerDto>(economicCustomer);
        //                input.Password = "Dummy@123";
        //                input.EmailAddress = !string.IsNullOrWhiteSpace(input.EmailAddress) ? input.EmailAddress : (input.CustomerNo + "@e-conomic.com");
        //                input.UserName = input.EmailAddress;

        //                var newUser = new User();
        //                if (customerDbo == null)
        //                {
        //                    var userDbo = _userManager.Users.Where(x => x.TenantId == args.TenantId && x.EmailAddress == input.EmailAddress).FirstOrDefault();
        //                    if (userDbo == null)
        //                    {
        //                        newUser = _objectMapper.Map<User>(input);
        //                        newUser.Name = newUser.Name.Truncate(20);
        //                        newUser.Surname = newUser.Surname?.Truncate(20) ?? "";
        //                        newUser.TenantId = args.TenantId;
        //                        newUser.UserTypeId = userTypeId;
        //                        newUser.IsActive = true;
        //                        await _userManager.CreateAsync(newUser, input.Password);
        //                       // await _userManager.AddToRoleAsync(newUser, "ADMIN");
        //                        _unitOfWorkManager.Current.SaveChanges();
        //                        newUser = _userManager.Users.OrderByDescending(x => x.Id).FirstOrDefault();
        //                    }
        //                    //else
        //                    //{
        //                    //    newUser = userDbo;
        //                    //}
        //                    //if (_customerRepository.FirstOrDefault(x => x.UserId == newUser.Id) == null)
        //                    //{
        //                    //    var customer = Customer.Create(args.TenantId,
        //                    //                               input.CustomerNo,
        //                    //                               input.Address ?? "",
        //                    //                               input.Postcode ?? "",
        //                    //                               input.TownCity ?? "",
        //                    //                               input.Country ?? "",
        //                    //                               input.TelephoneFax ?? "",
        //                    //                               input.Website ?? "",
        //                    //                               input.Currency ?? "",
        //                    //                               newUser.Id,
        //                    //                               newUser.Id,
        //                    //                               1,
        //                    //                               null, false, string.Empty);
        //                    //    _customerRepository.Insert(customer);
        //                    //}
        //                }
        //                unitOfWork.Complete();
        //            }
        //            index++;
        //        };
        //    }
        //    else
        //    {
        //        using (var unitOfWork = _unitOfWorkManager.Begin())
        //        {
        //            history.IsFailed = true;
        //            _economicSyncHistoryRepository.Update(history);
        //            unitOfWork.Complete();
        //        }
        //    }
        //}



        private const int _defaultPageSize = 100; // Define default page size

        private async Task SyncCustomers(DataImportJobInputDto args, Company company, EconomicSyncHistory history, string lastCustomerNo)
        {
            try
            {
                var pageIndex = 0;
                var allEconomicCustomers = new List<EconomicCustomerDto>();
                RestResponse response;
                EconomicApiResponseDto<EconomicCustomerDto> result = null;
                do
                {
                    response = await GetEconomicCustomers(pageIndex, company, lastCustomerNo);

                    if (!response.IsSuccessful)
                    {
                        await HandleSyncFailure(history);
                        return;
                    }

                    // Deserialize the API response
                     result = JsonSerializer.Deserialize<EconomicApiResponseDto<EconomicCustomerDto>>(response.Content);

                    if (result?.collection != null && result?.collection.Count() > 0)
                    {
                        allEconomicCustomers.AddRange(result.collection);
                    }

                    pageIndex++;
                } while (result?.pagination.results > (pageIndex * _defaultPageSize)); // Fixed _defaultPageSize usage

                // Process customers in parallel
                var tasks = allEconomicCustomers.Select(async economicCustomer =>
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        await ProcessEconomicCustomer(args, economicCustomer,history);
                        unitOfWork.Complete();
                    }
                });

                await Task.WhenAll(tasks); // Wait for all customer processing tasks to complete
            }
            catch (Exception ex)
            {
                // Log error
                await HandleSyncFailure(history, ex);
            }
        }

        private async Task ProcessEconomicCustomer(DataImportJobInputDto args, EconomicCustomerDto economicCustomer, EconomicSyncHistory history)
        {
           // Check if customer already exists in the database based on TenantId and CustomerNo
           var customerDbo = _customerRepository
               .GetAll()
               .FirstOrDefault(x => x.TenantId == args.TenantId && x.CustomerNo == economicCustomer.customerNumber.ToString());

            // If the customer already exists, skip processing
            if (customerDbo != null)
            {
                return; // Exit the method early if the customer is already synced
            }

           // Map EconomicCustomerDto to CreateCustomerDto
            var input = _objectMapper.Map<CreateCustomerDto>(economicCustomer);
            input.Password = "Dummy@123"; // Default password for new users
            input.EmailAddress = !string.IsNullOrWhiteSpace(input.EmailAddress)
                ? input.EmailAddress
                : $"{input.CustomerNo+args.TenantId}@e-conomic.com";
            input.UserName = input.EmailAddress;

            User newUser = null; // Variable to hold the new user

            // Check if a user with the same email already exists
            var userDbo = _userManager.Users .FirstOrDefault(x =>  x.EmailAddress == input.EmailAddress);

            if (userDbo == null)
            {
                // User doesn't exist, create a new user
                newUser = _objectMapper.Map<User>(input);
                newUser.Name = newUser.Name.Truncate(20);
                newUser.Surname = newUser.Surname?.Truncate(20) ?? "";
                newUser.TenantId = args.TenantId;
                newUser.UserTypeId = userTypeId; // Ensure userTypeId is set correctly elsewhere
                newUser.IsActive = true;

                // Create and save the new user
                await _userManager.CreateAsync(newUser, input.Password);
                await _unitOfWorkManager.Current.SaveChangesAsync();

                // Fetch the newly created user
                newUser = _userManager.Users.OrderByDescending(x => x.Id).FirstOrDefault();
            }
            else
            {
                // If user exists, assign the existing user to newUser
                newUser = userDbo;
            }

            var historyDetail = SyncHistoryDetail.Create(args.TenantId, "Customer", newUser.Id.ToString(), newUser.Name, history.Id);
            _syncHistoryDetailRepository.Insert(historyDetail);

            // Check if the customer is already associated with the user
            if (_customerRepository.FirstOrDefault(x => x.UserId == newUser.Id) == null)
            {
                // Create and insert the new customer
                var customer = Customer.Create(args.TenantId,
                    input.CustomerNo,
                    input.Address ?? "",
                    input.Postcode ?? "",
                    input.TownCity ?? "",
                    input.Country ?? "",
                    input.TelephoneFax ?? "",
                    input.Website ?? "",
                    input.Currency ?? "",
                    newUser.Id, // Associate the user with the customer
                    newUser.Id,
                    1,
                    null,
                    false,
                    string.Empty);

                _customerRepository.Insert(customer);
            }
        }


        private async Task HandleSyncFailure(EconomicSyncHistory history, Exception ex = null)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                history.IsFailed = true;
                history.ErrorLog = ex.Message;
                _economicSyncHistoryRepository.Update(history);
                await unitOfWork.CompleteAsync();
            }

            if (ex != null)
            {
                // Log exception (e.g., _logger.LogError(ex, "SyncCustomers failed"))
            }
        }






        private async Task<RestResponse> GetEconomicCustomers(int pageIndex, Company company, string lastNumber = "")
        {
            DateTime dateFilter = new DateTime(2024, 1, 1);
            var url = _BaseUrl + "customers?skippages=" + pageIndex + "&pagesize=" + _defaulPageSize;
            if (!string.IsNullOrWhiteSpace(lastNumber))
            {
                url += "&filter=customerNumber$gt:";
                url += lastNumber;
            }
            url += "$and:lastUpdated$gte:";
            url += dateFilter;
            var client = new RestClient(url);
            var request = new RestRequest();
            request.AddHeader("X-AppSecretToken", company.EconomicAppSecretToken);
            request.AddHeader("X-AgreementGrantToken", company.EconomicAgreementGrantToken);
            var response = await client.ExecuteAsync(request);
            return response;
        }

        private async Task<RestResponse> GetEconomicProduct(string productNumber, Company company)
        {
            var client = new RestClient(_BaseUrl + "products/" + productNumber);
            var request = new RestRequest();
            request.AddHeader("X-AppSecretToken", company.EconomicAppSecretToken);
            request.AddHeader("X-AgreementGrantToken", company.EconomicAgreementGrantToken);
            var response = await client.ExecuteAsync(request);
            return response;
        }
        private async Task<RestResponse> GetEconomicInvoicelines(string invoiceNumber, Company company)
        {
            var client = new RestClient(_BaseUrl + "invoices/booked/" + invoiceNumber);
            var request = new RestRequest();
            request.AddHeader("X-AppSecretToken", company.EconomicAppSecretToken);
            request.AddHeader("X-AgreementGrantToken", company.EconomicAgreementGrantToken);
            var response = await client.ExecuteAsync(request);
            return response;
        }
        private async Task<RestResponse> GetEconomicInvoices(int pageIndex, Company company, string lastNumber = "")
        {
            var url = _BaseUrl + "invoices/booked?skippages=" + pageIndex + "&pagesize=" + _defaulPageSize;
            if (!string.IsNullOrWhiteSpace(lastNumber))
            {
                url += "&filter=bookedInvoiceNumber$gt:";
                url += lastNumber;
            }
            var client = new RestClient(url);

            var request = new RestRequest();
            request.AddHeader("X-AppSecretToken", company.EconomicAppSecretToken);
            request.AddHeader("X-AgreementGrantToken", company.EconomicAgreementGrantToken);
            var response = await client.ExecuteAsync(request);
            return response;
        }

        private Invoice MapEconomicInvoiceToInvoice(EconomicInvoiceDto obj, int tenantId, long userId, int customerId)
        {
            return Invoice.Create(tenantId,
                                  obj.bookedInvoiceNumber.ToString(),
                                  obj.date,
                                  obj.dueDate,
                                  obj.currency ?? "",
                                  Convert.ToDecimal(obj.netAmount),
                                  "",
                                  customerId,
                                  true);
        }
        private static List<Customer> MapEconomicCustomersToCustomers(List<EconomicCustomerDto> objs, int tenantId)
        {
            var customers = new List<Customer>();
            objs.ForEach(obj =>
            {
                customers.Add(MapEconomicCustomerToCustomer(obj, tenantId));
            });
            return customers;
        }

        private static Customer MapEconomicCustomerToCustomer(EconomicCustomerDto obj, int tenantId)
        {
            return Customer.Create(tenantId, obj.customerNumber.ToString(), obj.address ?? "", obj.pNumber ?? "", obj.city ?? "", obj.country ?? "", obj.telephoneAndFaxNumber ?? "", obj.website ?? "", obj.currency ?? "", 0, null, 1, null, false, string.Empty);
        }

        private static Activity GetActivityModel(CreateActivityDto activity, int tenantId)
        {
            return Activity.Create(tenantId,null, activity.Name,activity.GroupId, DateTime.ParseExact(activity.Date, OpticianConsts.DateFormate, CultureInfo.InvariantCulture), DateTime.ParseExact(activity.FollowUpDate, OpticianConsts.DateFormate, CultureInfo.InvariantCulture), activity.ActivityTypeId, activity.FollowUpTypeId, activity.ActivityArtId, activity.EmployeeId, activity.CustomerId, null);
        }
        private async Task<RestResponse> GetEconomicProducts(int pageIndex, Company user, string lastProductNo)
        {
            var url = _BaseUrl + "products?skippages=" + pageIndex + "&pagesize=" + _defaulPageSize;
            if (!string.IsNullOrWhiteSpace(lastProductNo))
            {
                url += "&filter=productNumber$gt:";
                url += lastProductNo;
            }
            var client = new RestClient(url);
            var request = new RestRequest();
            request.AddHeader("X-AppSecretToken", user.EconomicAppSecretToken);
            request.AddHeader("X-AgreementGrantToken", user.EconomicAgreementGrantToken);
            var response = await client.ExecuteAsync(request);
            return response;
        }

        private async Task<RestResponse> GetEconomicProductGroups(int pageIndex, Company user, string lastProductGroupNo)
        {
            var url = _BaseUrl + "product-groups?skippages=" + pageIndex + "&pagesize=" + _defaulPageSize;
            if (!string.IsNullOrWhiteSpace(lastProductGroupNo))
            {
                url += "&filter=productGroupNumber$gt:";
                url += lastProductGroupNo;
            }
            var client = new RestClient(url);
            var request = new RestRequest();
            request.AddHeader("X-AppSecretToken", user.EconomicAppSecretToken);
            request.AddHeader("X-AgreementGrantToken", user.EconomicAgreementGrantToken);
            var response = await client.ExecuteAsync(request);
            return response;
        }

        private static List<Product> MapEconomicProductsToProducts(List<EconomicProductDto> objs, int tenantId, long userId)
        {
            var Products = new List<Product>();


            objs.ForEach(obj =>
            {
                Products.Add(MapEconomicProductToProduct(obj, tenantId, userId));
            });

            return Products;
        }
        private static List<ProductGroup> MapEconomicProductGroupsToProductGroups(List<EconomicProductGroupDto> objs, int tenantId, long userId)
        {
            var ProductGroups = new List<ProductGroup>();
            objs.ForEach(obj =>
            {
                ProductGroups.Add(MapEconomicProductGroupToProductGroup(obj, tenantId, userId));
            });
            return ProductGroups;
        }

        private static Product MapEconomicProductToProduct(EconomicProductDto obj, int tenantId, long userId)
        {
            return new Product
            {
                Name = obj.name,
                TenantId = tenantId,
                ProductGroupNumber = obj.productGroup.productGroupNumber,
                SalesPrice = obj.salesPrice,
                Recprice = obj.recommendedPrice.ToString(),
                ProductNumber = obj.productNumber.ToString(),
            };
        }
        private static ProductGroup MapEconomicProductGroupToProductGroup(EconomicProductGroupDto obj, int tenantId, long userId)
        {
            return new ProductGroup
            {
                Name = obj.name,
                TenantId = tenantId,
                ProductGroupNumber = obj.productGroupNumber
            };
        }
    }

}
