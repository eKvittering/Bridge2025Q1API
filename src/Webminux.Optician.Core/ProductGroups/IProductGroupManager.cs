using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.ProductGroups
{
    public interface IProductGroupManager : IDomainService
    {

        Task CreateAsync(ProductGroup input);
        Task UpdateAsync(ProductGroup input);
        Task DeleteAsync(ProductGroup input);
        Task<ProductGroup> GetAsync(int id);
        IQueryable<ProductGroup> GetAll();
    }
}
