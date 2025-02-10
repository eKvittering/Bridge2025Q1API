using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.ProductGroups
{
    public class ProductGroupManager : IProductGroupManager
    {
        private readonly IRepository<ProductGroup> _productGroupRepository;

        public ProductGroupManager(IRepository<ProductGroup> productGroupRepository)
        {
            _productGroupRepository = productGroupRepository;
        }

        public async Task CreateAsync(ProductGroup input)
        {
            await _productGroupRepository.InsertAsync(input);
        }

        public async Task DeleteAsync(ProductGroup input)
        {
            await _productGroupRepository.DeleteAsync(input);
        }

        public  IQueryable<ProductGroup> GetAll()
        {
            return  _productGroupRepository.GetAll();
        }

        public async Task<ProductGroup> GetAsync(int id)
        {
            try
            {
                var data = await _productGroupRepository.GetAsync(id);
                return data;
            }
            catch (Exception )
            {

                throw;
            }
          
        }

        public async Task UpdateAsync(ProductGroup input)
        {
             await _productGroupRepository.UpdateAsync(input);
        }
    }
}
