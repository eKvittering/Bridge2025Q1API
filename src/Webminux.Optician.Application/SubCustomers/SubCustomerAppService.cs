using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Core.Customers;
using Webminux.Optician.Core.SubCustomers;
using Webminux.Optician.Customers.Dtos;
using Webminux.Optician.CustomFields;
using Webminux.Optician.Helpers;
using Webminux.Optician.SubCustomers.Dtos;
using Webminux.Optician.Users;
using static Webminux.Optician.OpticianConsts;

namespace Webminux.Optician.Application.SubCustomers
{
    /// <summary>
    /// This is an application service class for <see cref="SubCustomer"/> entity.
    /// </summary>
    public class SubCustomerAppService : OpticianAppServiceBase, ISubCustomerAppService
    {
        private readonly IRepository<SubCustomer> _subCustomerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubCustomerAppService"/> class.
        /// </summary>
        public SubCustomerAppService(IRepository<SubCustomer> subCustomerRepository)
        {
            _subCustomerRepository = subCustomerRepository;
        }

        /// <summary>
        /// This method creates a new sub customer.
        /// </summary>
        public async Task CreateAsync(SubCustomerDto input)
        {
            var tenantId = AbpSession.TenantId.Value;
            var subCustomer = SubCustomer.Create(tenantId, input.Name, input.Email, input.Phone, input.Address, input.CustomerId);
            await _subCustomerRepository.InsertAsync(subCustomer);
        }

        /// <summary>
        /// This method delete a sub customer.
        /// </summary>
        public async Task DeleteAsync(EntityDto input)
        {
            var subCustomer = await _subCustomerRepository.GetAsync(input.Id);
            if (subCustomer == null)
                throw new EntityNotFoundException(typeof(Customer), input.Id);

            await _subCustomerRepository.DeleteAsync(subCustomer);
        }

        /// <summary>
        /// This method gets  all sub customer.
        /// </summary>
        public async Task<ListResultDto<SubCustomerDto>> GetAllAsync()
        {
            var query = _subCustomerRepository.GetAll();
            var subCustomers = await GetSelectQuery(query).ToListAsync();
            return new ListResultDto<SubCustomerDto>(subCustomers);
        }

        /// <summary>
        /// Get Sub Customers of specific customer 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ListResultDto<SubCustomerDto>> GetAllOfCustomerAsync(EntityDto input)
        {
            var query = _subCustomerRepository.GetAll();
            query = query.Where(c => c.CustomerId == input.Id);
            var subCustomers = await GetSelectQuery(query).ToListAsync();
            return new ListResultDto<SubCustomerDto>(subCustomers);
        }

        private static IQueryable<SubCustomerDto> GetSelectQuery(IQueryable<SubCustomer> query)
        {
            return query.Select(c => new SubCustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                Address = c.Address,
                CustomerId = c.CustomerId,
                CreationTime = c.CreationTime,
                CreatorUserId = c.CreatorUserId
            });
        }

        #region GetAsync
        /// <summary>
        /// This method gets a customer by id.
        /// </summary>
        public async Task<SubCustomerDto> GetAsync(EntityDto input)
        {
            var subCustomerFromDb = await _subCustomerRepository.GetAsync(input.Id);
            if (subCustomerFromDb == null)
                throw new EntityNotFoundException(typeof(Customer), input.Id);

            var subCustomer = ObjectMapper.Map<SubCustomerDto>(subCustomerFromDb);
            return subCustomer;
        }
        #endregion

        #region  GetPagedResultAsync
        /// <summary>
        /// This method gets paged result of customer with PagedCustomerResultRequestDto input.
        /// </summary>
        public async Task<PagedResultDto<SubCustomerDto>> GetPagedResultAsync(PagedSubCustomerResultRequestDto input)
        {
            var query = _subCustomerRepository.GetAll();

            query = ApplyFilter(input, query);
            IQueryable<SubCustomerDto> selectQuery = GetSelectQuery(query);
            var pagedResult = await selectQuery.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
            return pagedResult;
        }

        private static IQueryable<CustomerDto> GetSelectQuery(IQueryable<Customer> query)
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
                FullName = c.User.FullName,
                EmailAddress = c.User.EmailAddress,
                CreationTime = c.User.CreationTime,
                UserTypeId = c.User.UserTypeId
            });
        }

        private static IQueryable<SubCustomer> ApplyFilter(PagedSubCustomerResultRequestDto input, IQueryable<SubCustomer> query)
        {
            if (string.IsNullOrWhiteSpace(input.Keyword) == false)
            {
                query = query.Where(
                    a => a.Name.Contains(input.Keyword)
                                || a.Email.Contains(input.Keyword)
                                || a.Phone.Contains(input.Keyword));
            }
            query = query.Where(c => c.CustomerId == input.CustomerId);

            return query;
        }

        #endregion

        #region  UpdateAsync
        /// <summary>
        /// This method updates a sub customer.
        /// </summary>
        public async Task UpdateAsync(SubCustomerDto input)
        {

            var subCustomer = await _subCustomerRepository.GetAsync(input.Id);
            if (subCustomer == null)
                throw new EntityNotFoundException(typeof(Customer), input.Id);

            ObjectMapper.Map(input, subCustomer);
        }
        #endregion
    }
}
