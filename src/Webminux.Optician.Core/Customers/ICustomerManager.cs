using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Services;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Core.Customers;

public interface ICustomerManager : IDomainService
{
    Task<long> CreateAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(Customer customer);
    Task<Customer> GetAsync(int id);
    Task<User> GetUserAsync(int id);

    IQueryable<Customer> Customers { get; }
}