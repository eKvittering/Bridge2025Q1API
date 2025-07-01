using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webminux.Optician.Application.Customers;
using Webminux.Optician.Application.Customers.Dtos;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Core;
using Webminux.Optician.Core.Customers;
using Webminux.Optician.Customers.Dtos;
using Webminux.Optician.CustomFields;
using Webminux.Optician.Helpers;
using Webminux.Optician.MultiTenancy;
using Webminux.Optician.Users;
using static Webminux.Optician.OpticianConsts;

namespace Webminux.Optician.Customers
{
    /// <summary>
    /// This is an application service class for <see cref="Customer"/> entity.
    /// </summary>
    public class CustomerAppService : OpticianAppServiceBase, ICustomerAppService
    {
        private readonly IUserAppService _userAppService;
        private readonly ICustomerManager _customerManager;
        private readonly UserManager _userManager;
        private readonly IRepository<CustomerType> _customerTypeRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IRepository<User, long> repository, _userRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly ICustomFieldManager _customFieldManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerAppService"/> class.
        /// </summary>
        public CustomerAppService(IUserAppService userAppService,
        ICustomerManager customerManager,
        IRepository<CustomerType> customerTypeRepository,
        ICustomFieldManager customFieldManager,
        IRepository<Customer> customerRepository,
        IRepository<Tenant> tenantRepository,
        IRepository<User, long> userRepository,
        IUnitOfWorkManager unitOfWorkManager)
        {
            _userAppService = userAppService;
            _customerManager = customerManager;
            _customerTypeRepository = customerTypeRepository;
            _customFieldManager = customFieldManager;
            _customerRepository = customerRepository;
            _tenantRepository = tenantRepository;
            _userRepository = userRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <summary>
        /// This method creates a new customer.
        /// </summary>
        [AbpAllowAnonymous]
        public async Task<long> CreateAsync(CreateCustomerDto input)
        {
            try
            {
                int tenantId = AbpSession.TenantId.HasValue && AbpSession.TenantId.Value > 0
                    ? AbpSession.TenantId.Value
                    : input.TenantId;

                // Ensure RoleNames has at least "Admin"
                if (!input.RoleNames.Any())
                {
                    input.RoleNames = new string[] { "Admin" };
                }

                // Create the user
                Users.Dto.UserDto user = await _userAppService.CreateAsync(input);

                // Create the customer entity
                Customer customer = Customer.Create(
                    tenantId == 0 ? 1 : tenantId,
                    input.CustomerNo, input.Address, input.Postcode, input.TownCity,
                    input.Country, input.TelephoneFax, input.Website, input.Currency, user.Id,
                    input.ResponsibleEmployeeId, input.CustomeTypeId, input.ParentId,
                    input.IsSubCustomer, input.Site
                );

                // Save customer
                long customerUserId = await _customerManager.CreateAsync(customer);
                await UnitOfWorkManager.Current.SaveChangesAsync();

                // Create entity field mappings
                await _customFieldManager.CreateEntityFieldMappings(input.CustomFields, tenantId, customer.Id);

                return customerUserId;
            }
            catch (UserFriendlyException ex)
            {
                // Handle known errors
                Logger.Error($"User-friendly error: {ex.Message}", ex);
                throw new UserFriendlyException("An error occurred while creating the customer. Please try again.", ex);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                Logger.Error($"Unexpected error in CreateAsync: {ex.Message}", ex);
                throw new UserFriendlyException("A system error occurred. Please contact support.", ex);
            }
        }


        /// <summary>
        /// This method delete a customer.
        /// </summary>
        public async Task DeleteAsync(EntityDto input)
        {
            Customer customer = await _customerManager.GetAsync(input.Id);
            if (customer == null)
            {
                throw new EntityNotFoundException(typeof(Customer), input.Id);
            }

            await _customerManager.DeleteAsync(customer);
        }

        /// <summary>
        /// This method gets  all customer.
        /// </summary>

        public async Task<ListResultDto<CustomerListDto>> GetAllAsync()
        {
            var info = AbpSession;
            System.Collections.Generic.List<CustomerListDto> customers = await _customerManager.Customers.Select(c => new CustomerListDto
            {
                Id = c.Id,
                CustomerNo = c.CustomerNo,
                Name = c.User.Name,
                EmailAddress = c.User.EmailAddress,
                CustomerUserId = c.UserId,
                TelephoneFax = c.TelephoneFax,
                TenantId = c.TenantId

            }).ToListAsync();

            return new ListResultDto<CustomerListDto>(customers);
        }


        //public async Task<ListResultDto<CustomerListDto>> GetAllAsync()
        //{
        //    var info = AbpSession;
        //    using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MustHaveTenant))
        //    {
        //        using (CurrentUnitOfWork.SetTenantId(null))
        //        {
        //            var query = from customer in _customerRepository.GetAll()
        //                        join user in _userRepository.GetAll() on customer.UserId equals user.Id
        //                        select new CustomerListDto
        //                        {
        //                            Id = customer.Id,
        //                            CustomerNo = customer.CustomerNo,
        //                            Name = user.Name,
        //                            EmailAddress = user.EmailAddress,
        //                            CustomerUserId = customer.UserId,
        //                            TelephoneFax = customer.TelephoneFax,
        //                            TenantId = customer.TenantId
        //                        };

        //            var customers = await query.ToListAsync();
        //            return new ListResultDto<CustomerListDto>(customers);

        //        }
        //    }
        //}


        /// <summary>
        /// GetAllLazyLoadingAsync
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<CustomerListDto>> GetAllLazyLoadingAsync(string searchTerm, int pageNumber = 1, int pageSize = 20)
        {

            try
            {

                //var query = _customerManager.Customers;
                // Get the base customer query and only include necessary fields
                IQueryable<Customer> query = _customerManager.Customers
                    .Include(c => c.User)
                    .AsQueryable();

                var input = new PagedCustomerResultRequestDto();
                input.Keyword = searchTerm;
                input.showAllSubCustomers = false;
                query = ApplyFilter(input, query);
                query = query.OrderByDescending(c => c.Id);
                IQueryable<CustomerDto> selectQuery = GetSelectQuery(query);
                int totalCount = await query.CountAsync();

                // Apply paging (Skip/Take)
                var customers = await selectQuery.Select(c => new CustomerListDto
                {
                    Id = c.Id,
                    CustomerNo = c.CustomerNo,
                    Name = c.FullName,
                    EmailAddress = c.EmailAddress,
                    CustomerUserId = c.UserId,
                    Address = c.Address,
                    Postcode = c.Postcode,
                    TownCity = c.TownCity,
                    Country = c.Country,
                    TelephoneFax = c.TelephoneFax,
                    Website = c.Website,
                    Currency = c.Currency,
                    UserId = c.UserId,
                    UserName = c.UserName,
                    Surname = c.Surname,
                    UserTypeId = c.UserTypeId

                })
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize).ToListAsync();

                // Return paginated result
                return new PagedResultDto<CustomerListDto>(totalCount, customers);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region GetAsync
        /// <summary>
        /// This method gets a customer by id.
        /// </summary>
        public async Task<CustomerDto> GetAsync(EntityDto input)
        {
            try
            {
                var customer = await _customerManager.GetAsync(input.Id);

                if (customer == null)
                {
                    throw new EntityNotFoundException(typeof(Customer), input.Id);
                }

                CustomerDto customerToReturn = GetCustomerDtoFromCustomer(customer);

                EntityDto<long> getUserInputDto = new() { Id = customer.UserId };
                Users.Dto.UserDto user = await _userAppService.GetAsync(getUserInputDto);

                if (user == null)
                {
                    throw new EntityNotFoundException(typeof(User), customer.UserId);
                }

                FillUserInformation(customerToReturn, user);

                await GetCustomerCustomFieldsAndAssignToCustomerDto(customer.Id, customerToReturn);
                return customerToReturn;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task GetCustomerCustomFieldsAndAssignToCustomerDto(int customerId, CustomerDto customerToReturn)
        {
            int tenantId = AbpSession.TenantId ?? DefaultTenantId;
            //List<EntityFieldMappingDto> customerEntityCustomFields = await GetCustomerCustomFields(customerId);
            System.Collections.Generic.List<EntityFieldMappingDto> customerEntityCustomFields = await _customFieldManager.GetObjectMappedCustomFields(customerId, Screen.Customer);
            System.Collections.Generic.List<EntityFieldMappingDto> customerScreenCustomFields = await _customFieldManager.GetScreenCustomFields((int)Screen.Customer);

            if (_customFieldManager.IsEntityHasNoCustomFieldsOrScreenHasNewCustomFields(customerEntityCustomFields, customerScreenCustomFields))
            {
                if (_customFieldManager.IsHasCustomFields(customerEntityCustomFields))
                {
                    System.Collections.Generic.IEnumerable<int> customerEntityCustomFieldIds = customerEntityCustomFields.Select(f => f.CustomFieldId);
                    System.Collections.Generic.List<EntityFieldMappingDto> newCustomFields = customerScreenCustomFields.Where(f => customerEntityCustomFieldIds.Contains(f.CustomFieldId) == false).ToList();
                    _customFieldManager.InitializeFieldValueWithEmptyString(newCustomFields);
                    // await CreateEntityFieldMappings(newCustomFields, tenantId, customerId);
                    await _customFieldManager.CreateEntityFieldMappings(newCustomFields, tenantId, customerId);
                }
                else if (_customFieldManager.IsHasCustomFields(customerScreenCustomFields))
                {
                    //InitializeFieldValueWithEmptyString(customerScreenCustomFields);
                    _customFieldManager.InitializeFieldValueWithEmptyString(customerScreenCustomFields);
                    // await CreateEntityFieldMappings(customerScreenCustomFields, tenantId, customerId);
                    await _customFieldManager.CreateEntityFieldMappings(customerScreenCustomFields, tenantId, customerId);
                }
                await CurrentUnitOfWork.SaveChangesAsync();
                //customerToReturn.CustomFields = await GetCustomerCustomFields(customerId);
                customerToReturn.CustomFields = await _customFieldManager.GetObjectMappedCustomFields(customerId, Screen.Customer);
            }
            else
            {
                customerToReturn.CustomFields = customerEntityCustomFields;
            }
        }

        public async Task<GetCustomerNoResultDto> GetNextCustomerNoAsync()
        {
            int customerCount = await _customerManager.Customers.CountAsync();
            int customerNo = customerCount + 1;
            return new GetCustomerNoResultDto { Number = customerNo };
        }

        private static void FillUserInformation(CustomerDto customerToReturn, Users.Dto.UserDto user)
        {
            customerToReturn.UserTypeId = user.UserTypeId;
            customerToReturn.FullName = user.FullName;
            customerToReturn.RoleNames = user.RoleNames;
            customerToReturn.EmailAddress = user.EmailAddress;
            customerToReturn.IsActive = user.IsActive;
            customerToReturn.CreationTime = user.CreationTime;
            customerToReturn.LastLoginTime = user.LastLoginTime;
            customerToReturn.Name = user.Name;
            customerToReturn.Surname = user.Surname;
            customerToReturn.UserName = user.UserName;
            customerToReturn.UserId = user.Id;
        }

        private static CustomerDto GetCustomerDtoFromCustomer(Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                CustomerNo = customer.CustomerNo,
                Address = customer.Address,
                Postcode = customer.Postcode,
                TownCity = customer.TownCity,
                Country = customer.Country,
                TelephoneFax = customer.TelephoneFax,
                Website = customer.Website,
                Currency = customer.Currency,
                ResponsibleEmployeeId = customer.ResponsibleEmployeeId,
                ParentId = customer.ParentId,
                CustomerTypeId = customer.CustomeTypeId,
                IsSubCustomer = customer.IsSubCustomer,
                Site = customer.Site,
            };
        }
        #endregion

        #region  GetPagedResultAsync
        /// <summary>
        /// This method gets paged result of customer with PagedCustomerResultRequestDto input.


        public async Task<PagedResultDto<CustomerDto>> GetPagedResultAsync(PagedCustomerResultRequestDto input)
        {
            try
            {
                int tenantId = AbpSession.TenantId.HasValue && AbpSession.TenantId.Value > 0
                        ? AbpSession.TenantId.Value
                        : 1;
                string tenancyName = "";
                if (AbpSession.TenantId != null)
                {
                    var tenant = await _tenantRepository.FirstOrDefaultAsync(x => x.Id == AbpSession.TenantId);
                    if (tenant != null)
                        tenancyName = tenant.TenancyName;
                }

                if (AbpSession.TenantId == null || tenancyName=="5000")
                {
                    using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MustHaveTenant, AbpDataFilters.MayHaveTenant))
                    {


                        // Get the base customer query and only include necessary fields
                        IQueryable<Customer> query = _customerManager.Customers
                           .Include(c => c.User)
                           .AsQueryable();

                        // Apply filters before projections to reduce data load
                        query = ApplyFilter(input, query);
                        query = query.OrderByDescending(c => c.Id);
                        // Use the method to get the select query
                        IQueryable<CustomerDto> selectQuery = GetSelectQuery(query);
                        int totalCount = await query.CountAsync();

                        // Perform paging using Skip and Take
                        List<CustomerDto> pagedItems = await selectQuery
                            .Skip(input.SkipCount)
                            .Take(input.MaxResultCount)
                            .ToListAsync();

                        // Return paged result
                        return new PagedResultDto<CustomerDto>(totalCount, pagedItems);
                    }
                }
                else
                {
                    // Get the base customer query and only include necessary fields
                    IQueryable<Customer> query = _customerManager.Customers
                       .Include(c => c.User)
                       .AsQueryable();

                    // Apply filters before projections to reduce data load
                    query = ApplyFilter(input, query);
                    query = query.OrderByDescending(c => c.Id);
                    // Use the method to get the select query
                    IQueryable<CustomerDto> selectQuery = GetSelectQuery(query);
                    int totalCount = await query.CountAsync();

                    // Perform paging using Skip and Take
                    List<CustomerDto> pagedItems = await selectQuery
                        .Skip(input.SkipCount)
                        .Take(input.MaxResultCount)
                        .ToListAsync();

                    // Return paged result
                    return new PagedResultDto<CustomerDto>(totalCount, pagedItems);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error in getting paged result of customer", ex.Message);
            }
        }

        // New method to handle the select query logic
        private IQueryable<CustomerDto> GetSelectQuery(IQueryable<Customer> query)
        {
            return query.Select(c => new CustomerDto
            {
                Id = c.Id,
                CustomerNo = c.CustomerNo,
                Address = c.Address,
                Postcode = c.Postcode,
                TownCity = c.TownCity,
                Country = c.Country,
                TelephoneFax = c.TelephoneFax,
                Website = c.Website,
                Currency = c.Currency,
                UserId = c.UserId,
                UserName = c.User.UserName,
                Name = c.User.Name,
                Surname = c.User.Surname,
                FullName = c.User.FullName,
                EmailAddress = c.User.EmailAddress,
                CreationTime = c.User.CreationTime,
                UserTypeId = c.User.UserTypeId,
                ParentId = c.ParentId,
                Site = c.Site,
                CustomerTypeId = c.CustomeTypeId,
                CustomerType = c.CustomerType != null ? c.CustomerType.Type : string.Empty,
                ResponsibleEmployee = c.ResponsibleEmployee != null ? c.ResponsibleEmployee.FullName : string.Empty,
                IsSubCustomer = c.IsSubCustomer,
                TenantId = c.TenantId,
                //SubCustomers = c.SubCustomers.Select(s => new CustomerDto
                //{
                //    Id = s.Id,
                //    CustomerNo = s.CustomerNo,
                //    Address = s.Address,
                //    Postcode = s.Postcode,
                //    TownCity = s.TownCity,
                //    Country = s.Country,
                //    TelephoneFax = s.TelephoneFax,
                //    Website = s.Website,
                //    Currency = s.Currency,
                //    UserId = s.UserId,
                //    UserName = s.User.UserName,
                //    Name = s.User.Name,
                //    Surname = s.User.Surname,
                //    FullName = s.User.FullName,
                //    EmailAddress = s.User.EmailAddress,
                //    CreationTime = s.User.CreationTime,
                //    UserTypeId = s.User.UserTypeId,
                //    ParentId = s.ParentId,
                //    Site = s.Site,
                //    CustomerTypeId = s.CustomeTypeId,
                //    CustomerType = s.CustomerType != null ? s.CustomerType.Type : string.Empty,
                //    ResponsibleEmployee = s.ResponsibleEmployee != null ? s.ResponsibleEmployee.FullName : string.Empty,
                //    IsSubCustomer = s.IsSubCustomer
                //}).ToList()
            });
        }



        //public async Task<PagedResultDto<CustomerDto>> GetPagedResultAsync(PagedCustomerResultRequestDto input)
        //{
        //    try
        //    {
        //        IQueryable<Customer> query = from customers in _customerManager.Customers.Include(c => c.User)
        //                                     join sc in _customerManager.Customers.Include(c => c.User) on customers.Id equals sc.ParentId into subCustomers
        //                                     from subCustomer in subCustomers.DefaultIfEmpty()
        //                                     select customers;
        //        query = ApplyFilter(input, query);
        //        IQueryable<CustomerDto> selectQuery = GetSelectQuery(query);
        //        PagedResultDto<CustomerDto> pagedResult = await selectQuery.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
        //        return pagedResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException("Error in getting paged result of customer", ex.Message);
        //    }

        //}

        /// <summary>
        /// This method gets paged result of customer with PagedCustomerResultRequestDto input.
        /// </summary>
        public async Task<ListResultDto<CustomerDto>> GetSubCustomers(GetSubCustomersDto input)
        {
            IQueryable<Customer> query = _customerManager.Customers;

            if (string.IsNullOrWhiteSpace(input.Keyword) == false)
            {
                query = query.Where(
                    a => a.User.UserName.Contains(input.Keyword)
                                || a.User.UserType.Name.Contains(input.Keyword)
                                || a.User.EmailAddress.Contains(input.Keyword)
                                || string.Concat(a.User.Name, " ", a.User.Surname).Contains(input.Keyword)
                                || a.Country.Contains(input.Keyword)
                                || a.Address.Contains(input.Keyword)
                                || a.Currency.Contains(input.Keyword)
                                || a.CustomerNo.Contains(input.Keyword)
                                || a.TelephoneFax.Contains(input.Keyword)
                                || a.TownCity.Contains(input.Keyword)
                                );
            }

            query = query.Where(x => x.ParentId == input.CustomerId);
            IQueryable<CustomerDto> selectQuery = GetSelectQuery(query);
            System.Collections.Generic.List<CustomerDto> customers = await selectQuery.ToListAsync();
            return new ListResultDto<CustomerDto>(customers);
        }

        private IQueryable<Customer> ApplyFilter(PagedCustomerResultRequestDto input, IQueryable<Customer> query)
        {

            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                input.Keyword = input.Keyword.Trim().ToLower();
                query = query.Where(a =>
                    a.User.UserName.Trim().ToLower().Contains(input.Keyword) ||
                    a.User.UserType.Name.Trim().ToLower().Contains(input.Keyword) ||
                    a.User.EmailAddress.Trim().ToLower().Contains(input.Keyword) ||
                    string.Concat(a.User.Name, " ", a.User.Surname).Trim().ToLower().Contains(input.Keyword) ||
                    a.Country.Trim().ToLower().Contains(input.Keyword) ||
                    a.Address.Trim().ToLower().Contains(input.Keyword) ||
                    a.Currency.Trim().ToLower().Contains(input.Keyword) ||
                    a.CustomerNo.Trim().ToLower().Contains(input.Keyword) ||
                    a.TelephoneFax.Trim().ToLower().Contains(input.Keyword) ||
                    a.TownCity.Trim().ToLower().Contains(input.Keyword) ||
                    a.Id.ToString().Contains(input.Keyword) ||
                    a.UserId.ToString().Contains(input.Keyword) ||
                    a.Site.Trim().ToLower().Contains(input.Keyword));


                if (input.showAllSubCustomers)
                {
                    query = query.Where(a => a.SubCustomers.Any(s =>
                        s.User.UserName.Trim().ToLower().Contains(input.Keyword) ||
                        s.User.UserType.Name.Trim().ToLower().Contains(input.Keyword) ||
                        s.User.EmailAddress.Trim().ToLower().Contains(input.Keyword) ||
                        string.Concat(s.User.Name.Trim().ToLower(), " ", s.User.Surname.Trim().ToLower()).Contains(input.Keyword) ||
                        s.Country.Trim().ToLower().Contains(input.Keyword) ||
                        s.Address.Contains(input.Keyword) ||
                        s.Currency.Trim().ToLower().Contains(input.Keyword) ||
                        s.CustomerNo.Trim().ToLower().Contains(input.Keyword) ||
                        s.TelephoneFax.Trim().ToLower().Contains(input.Keyword) ||
                        s.TownCity.Trim().ToLower().Contains(input.Keyword) ||
                        s.Site.Trim().ToLower().Contains(input.Keyword)));
                }
            }

            if (input.CustomerUserId > 0)
            {
                query = query.Where(c => c.UserId == input.CustomerUserId);
            }

            if (!string.IsNullOrWhiteSpace(input.CustomerNumber))
            {
                query = query.Where(a => a.CustomerNo.Contains(input.CustomerNumber));
            }

            if (!string.IsNullOrWhiteSpace(input.Name))
            {
                query = query.Where(a => string.Concat(a.User.Name, " ", a.User.Surname).Contains(input.Name));
            }

            if (!string.IsNullOrWhiteSpace(input.TelePhone))
            {
                query = query.Where(a => a.TelephoneFax.Contains(input.TelePhone));
            }

            if (!string.IsNullOrWhiteSpace(input.EmailAddress))
            {
                query = query.Where(a => a.User.EmailAddress.Contains(input.EmailAddress));
            }

            if (!string.IsNullOrWhiteSpace(input.SerialNumber))
            {
                query = query.Where(a => a.Invoices.Any(r => r.ProductItems.Any(p => p.SerialNumber == input.SerialNumber)));
            }

            if (input.ParentId.HasValue)
            {
                query = query.Where(p => p.ParentId == input.ParentId);
            }
            else
            {
                query = query.Where(p => p.ParentId == null);
            }


            if (input.CustomerTypeId.HasValue)
            {
                if (input.CustomerTypeId == 3)
                {

                    query = query.Where(p => _customerManager.Customers.Any(s => s.ParentId == p.Id));
                }
                else
                {
                    query = query.Where(c => c.CustomeTypeId == input.CustomerTypeId);
                }
            }

            if (input.ResponsibleEmployeeId > 0)
            {
                query = query.Where(c => c.ResponsibleEmployeeId == input.ResponsibleEmployeeId);
            }

            return query;
        }



        //private static IQueryable<CustomerDto> GetSelectQuery(IQueryable<Customer> query)
        //{
        //    return query.Select(c => new CustomerDto
        //    {
        //        Id = c.Id,
        //        CustomerNo = c.CustomerNo,
        //        Address = c.Address,
        //        Postcode = c.Postcode,
        //        TownCity = c.TownCity,
        //        Country = c.Country,
        //        TelephoneFax = c.TelephoneFax,
        //        Website = c.Website,
        //        Currency = c.Currency,
        //        UserId = c.UserId,
        //        UserName = c.User.UserName,
        //        Name = c.User.Name,
        //        Surname = c.User.Surname,
        //        FullName = c.User.FullName,
        //        EmailAddress = c.User.EmailAddress,
        //        CreationTime = c.User.CreationTime,
        //        UserTypeId = c.User.UserTypeId,
        //        ParentId = c.ParentId,
        //        Site = c.Site,
        //        CustomerTypeId = c.CustomeTypeId,
        //        CustomerType = c.CustomerType == null ? string.Empty : c.CustomerType.Type,
        //        ResponsibleEmployee = c.ResponsibleEmployee == null ? string.Empty : c.ResponsibleEmployee.FullName,
        //        IsSubCustomer = c.IsSubCustomer,
        //        SubCustomers = c.SubCustomers.Select(s => new CustomerDto
        //        {
        //            Id = s.Id,
        //            CustomerNo = s.CustomerNo,
        //            Address = s.Address,
        //            Postcode = s.Postcode,
        //            TownCity = s.TownCity,
        //            Country = s.Country,
        //            TelephoneFax = s.TelephoneFax,
        //            Website = s.Website,
        //            Currency = s.Currency,
        //            UserId = s.UserId,
        //            UserName = s.User.UserName,
        //            Name = s.User.Name,
        //            Surname = s.User.Surname,
        //            FullName = s.User.FullName,
        //            EmailAddress = s.User.EmailAddress,
        //            CreationTime = s.User.CreationTime,
        //            UserTypeId = s.User.UserTypeId,
        //            ParentId = s.ParentId,
        //            Site = s.Site,
        //            CustomerTypeId = s.CustomeTypeId,
        //            CustomerType = s.CustomerType == null ? string.Empty : s.CustomerType.Type,
        //            ResponsibleEmployee = s.ResponsibleEmployee == null ? string.Empty : s.ResponsibleEmployee.FullName,
        //            IsSubCustomer = s.IsSubCustomer
        //        }).ToList()
        //    }); ;
        //}

        //private IQueryable<Customer> ApplyFilter(PagedCustomerResultRequestDto input, IQueryable<Customer> query)
        //{
        //    query = query.Distinct();
        //    if (string.IsNullOrWhiteSpace(input.Keyword) == false)
        //    {
        //        query = query.Where(a =>
        //               a.User.UserName.Contains(input.Keyword) ||
        //               a.User.UserType.Name.Contains(input.Keyword) ||
        //               a.User.EmailAddress.Contains(input.Keyword) ||
        //               string.Concat(a.User.Name, " ", a.User.Surname).Contains(input.Keyword) ||
        //               a.Country.Contains(input.Keyword) ||
        //               a.Address.Contains(input.Keyword) ||
        //               a.Currency.Contains(input.Keyword) ||
        //               a.CustomerNo.Contains(input.Keyword) ||
        //               a.TelephoneFax.Contains(input.Keyword) ||
        //               a.TownCity.Contains(input.Keyword) ||
        //               a.Site.Contains(input.Keyword) ||
        //               a.SubCustomers.Any(s =>
        //                   s.User.UserName.Contains(input.Keyword) ||
        //                   s.User.UserType.Name.Contains(input.Keyword) ||
        //                   s.User.EmailAddress.Contains(input.Keyword) ||
        //                   string.Concat(s.User.Name, " ", s.User.Surname).Contains(input.Keyword) ||
        //                   s.Country.Contains(input.Keyword) ||
        //                   s.Address.Contains(input.Keyword) ||
        //                   s.Currency.Contains(input.Keyword) ||
        //                   s.CustomerNo.Contains(input.Keyword) ||
        //                   s.TelephoneFax.Contains(input.Keyword) ||
        //                   s.TownCity.Contains(input.Keyword) ||
        //                   s.Site.Contains(input.Keyword)
        //               )
        //           );


        //        if (!input.showAllSubCustomers)
        //        {
        //            // filter out the subcustomers which do not match the keyword



        //        }
        //    }

        //    if (input.CustomerUserId > 0)
        //    {
        //        query = query.Where(c => c.UserId == input.CustomerUserId);
        //    }
        //    if (!string.IsNullOrWhiteSpace(input.CustomerNumber))
        //    {
        //        query = query.Where(a => a.CustomerNo.Contains(input.CustomerNumber));
        //    }

        //    if (!string.IsNullOrWhiteSpace(input.Name))
        //    {
        //        query = query.Where(a => string.Concat(a.User.Name, " ", a.User.Surname).Contains(input.Name));
        //    }
        //    if (!string.IsNullOrWhiteSpace(input.TelePhone))
        //    {
        //        query = query.Where(a => a.TelephoneFax.Contains(input.TelePhone));
        //    }

        //    if (!string.IsNullOrWhiteSpace(input.EmailAddress))
        //    {
        //        query = query.Where(a => a.User.EmailAddress.Contains(input.EmailAddress));
        //    }
        //    if (!string.IsNullOrWhiteSpace(input.SerialNumber))
        //    {
        //        query = query.Where(a => a.Invoices.Any(r => r.ProductItems.Any(p => p.SerialNumber == input.SerialNumber)));
        //    }

        //    query = input.ParentId.HasValue ? query.Where(p => p.ParentId == input.ParentId) : query.Where(p => p.ParentId == null);

        //    if (input.CustomerTypeId.HasValue)
        //    {
        //        query = input.CustomerTypeId == 3
        //            ? query.Where(p => _customerManager.Customers.Count(s => s.ParentId == p.Id) > 0)
        //            : query.Where(c => c.CustomeTypeId == input.CustomerTypeId);
        //    }
        //    if (input.ResponsibleEmployeeId > 0)
        //    {
        //        query = query.Where(c => c.ResponsibleEmployeeId == input.ResponsibleEmployeeId);
        //    }

        //    return query;
        //}

        #endregion

        #region  UpdateAsync
        /// <summary>
        /// This method updates a customer.
        /// </summary>
        public async Task UpdateAsync(CustomerDto input)
        {
            await GetAndUpdateUser(input);

            Customer customer = await _customerManager.GetAsync(input.Id);
            if (customer == null)
            {
                throw new EntityNotFoundException(typeof(Customer), input.Id);
            }

            _ = ObjectMapper.Map(input, customer);
            await DeleteOldEntityFieldMappingsAsync(customer);
            // await CreateEntityFieldMappings(input.CustomFields, customer.TenantId, customer.Id);
            await _customFieldManager.CreateEntityFieldMappings(input.CustomFields, customer.TenantId, customer.Id);
        }
        private async Task GetAndUpdateUser(CustomerDto input)
        {
            Users.Dto.UserDto user = await _userAppService.GetAsync(new EntityDto<long> { Id = input.UserId });
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), input.UserId);
            }

            user.Name = input.Name;
            user.Surname = input.Surname;
            user.EmailAddress = input.EmailAddress;
            user.IsActive = input.IsActive;
            user.UserTypeId = input.UserTypeId;
            user.RoleNames = input.RoleNames;
            _ = await _userAppService.UpdateAsync(user);
        }

        private static Users.Dto.UserDto GetUserDtoFromCustomer(CustomerDto input, Customer customer)
        {
            return new Users.Dto.UserDto
            {
                Id = customer.UserId,
                Name = input.Name,
                Surname = input.Surname,
                EmailAddress = input.EmailAddress,
                IsActive = input.IsActive,
                UserTypeId = input.UserTypeId,
                RoleNames = input.RoleNames
            };
        }

        //private async Task CreateEntityFieldMappings(ICollection<EntityFieldMappingDto> customFields, int tenantId, int customerId)
        //{
        //    foreach (var field in customFields)
        //    {
        //        var entityFieldMapping = EntityFieldMapping.Create(tenantId, field.Value, customerId, field.CustomFieldId);
        //        //await _entityFieldMappingRepository.InsertAsync(entityFieldMapping);

        //    }
        //}

        private async Task DeleteOldEntityFieldMappingsAsync(Customer customer)
        {
            //List<EntityFieldMapping> oldCustomFields = await GetOldEntityFieldMappingsAsync(customer);
            System.Collections.Generic.List<EntityFieldMapping> oldCustomFields = await _customFieldManager.GetEntityFieldMappingsAsync(customer.Id);

            //foreach (var field in oldCustomFields)
            //{
            //    await _entityFieldMappingRepository.DeleteAsync(field);
            //}

            await _customFieldManager.DeleteEntityFieldMappingsAsync(oldCustomFields);
        }

        //private async Task<List<EntityFieldMapping>> GetOldEntityFieldMappingsAsync(Customer customer)
        //{
        //    return await _entityFieldMappingRepository.GetAll()
        //        .Where(field => field.ObjectId == customer.Id)
        //        .ToListAsync();
        //}

        #endregion

        #region GetCustomerId
        /// <summary>
        /// Provide customer table id against user ID
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EntityDto> GetCustomerIdFromUserIdAsync(EntityDto<long> input)
        {
            var customer = await _customerManager.Customers.Where(c => c.UserId == input.Id).Select(c => new { c.Id }).FirstOrDefaultAsync();
            return customer == null ? throw new UserFriendlyException("Provided Id is not valid") : new EntityDto(customer.Id);
        }
        #endregion

        #region CustomerTypes

        /// <summary>
        /// Get all activity types
        /// </summary>
        public async Task<ListResultDto<CustomerTypeDto>> GetCustomerTypes()
        {
            IQueryable<CustomerType> query = _customerTypeRepository.GetAll();

            System.Collections.Generic.List<CustomerTypeDto> result = await query.Select(s => new CustomerTypeDto { Id = s.Id, Type = s.Type }).ToListAsync();
            return new ListResultDto<CustomerTypeDto>(result);
        }

        #endregion
    }
}
