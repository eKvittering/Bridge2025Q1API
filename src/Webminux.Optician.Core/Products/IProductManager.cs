using Abp.Domain.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Webminux.Optician.Products
{
    public interface IProductManager :IDomainService
    {
        Task CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task<Product> GetAsync(int id);
        IQueryable<Product> GetAll();
    }
}
