using Abp.Domain.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Webminux.Optician.ProductItem
{
    public interface IProductItemManager : IDomainService
    {

        Task CreateAsync(ProductItem input);
        Task UpdateAsync(ProductItem input);
        Task DeleteAsync(ProductItem input);
        Task<ProductItem> GetAsync(int id);
        IQueryable<ProductItem> GetAll();
    }
}
