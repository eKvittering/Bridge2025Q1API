
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Castle.Core.Resource;
using Microsoft.Extensions.Caching.Distributed;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Core.Customers;

public class CustomerManager : ICustomerManager
{
    private readonly IRepository<Customer> _customerRepository;
    private readonly UserManager _userManager;
    private readonly IDistributedCache _distributedCache;

    private const string CustomerCacheKeyPrefix = "CustomerCache_";

    public CustomerManager(IRepository<Customer> customerRepository, UserManager userManager, IDistributedCache distributedCache)
    {
        _customerRepository = customerRepository;
        _userManager = userManager;
        _distributedCache = distributedCache;
    }

    public IQueryable<Customer> Customers => _customerRepository.GetAll();

    public async Task<long> CreateAsync(Customer customer)
    {
        var customerObject = await _customerRepository.InsertAsync(customer);
        return customerObject.UserId;
    }

    public async Task DeleteAsync(Customer customer)
    {
        await _customerRepository.DeleteAsync(customer);
        await _distributedCache.RemoveAsync(CustomerCacheKeyPrefix + customer.Id);
    }

    public async Task<User> GetUserAsync(int id)
    {
        return await _userManager.GetUserByIdAsync(id);
    }
    public async Task<Customer> GetAsync(int id)
    {
        var cacheKey = CustomerCacheKeyPrefix + id;

        var cachedCustomer = await _distributedCache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedCustomer))
        {
            return JsonSerializer.Deserialize<Customer>(cachedCustomer);
        }



        // Fetch the user from UserManager
        var user = await _userManager.GetUserByIdAsync(id);
        if (user == null)
        {
            throw new EntityNotFoundException(typeof(User), id);
        }

        // Fetch the related customer from the Customer table
        var customer = await _customerRepository
            .FirstOrDefaultAsync(c => c.Id == user.Id);

        // If no customer found, create a new customer object with user data
        if (customer == null)
        {
            customer = new Customer
            {
                Id = id,
                UserId = user.Id,
            };
        }
       
        if (customer != null)
        {
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Adjust the expiration policy
            await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(customer), options);
        }
        
        return customer;
    }

    public async Task UpdateAsync(Customer customer)
    {
        await _customerRepository.UpdateAsync(customer);
        await _distributedCache.RemoveAsync(CustomerCacheKeyPrefix + customer.Id);
    }
}


//using System.Linq;
//using System.Threading.Tasks;
//using Abp.Domain.Repositories;
//using Webminux.Optician.Authorization.Users;
//using Webminux.Optician.Core.Customers;

//public class CustomerManager : ICustomerManager
//{
//    private readonly IRepository<Customer> _customerRepository;
//    private readonly UserManager _userManager;

//    public CustomerManager(IRepository<Customer> customerRepository,UserManager userManager)
//    {
//        _customerRepository = customerRepository;
//        _userManager = userManager;
//    }
//    public IQueryable<Customer> Customers => _customerRepository.GetAll();

//    public async Task<long> CreateAsync(Customer customer)
//    {
//        var customerObject = await _customerRepository.InsertAsync(customer);
//        return customerObject.UserId;
//    }

//    public async Task DeleteAsync(Customer customer)
//    {
//        await _customerRepository.DeleteAsync(customer);
//    }

//    public async Task<Customer> GetAsync(int id)
//    {
//        var customer = await _customerRepository.GetAsync(id);
//        return customer;
//    }

//    public async Task UpdateAsync(Customer customer)
//    {
//        await _customerRepository.UpdateAsync(customer);
//    }
//}