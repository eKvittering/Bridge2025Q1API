using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.ProductItem
{
    public class ProductItemManager : IProductItemManager
    {
        private readonly IRepository<ProductItem> _ProductItemRepository;

        public ProductItemManager(IRepository<ProductItem> ProductItemRepository)
        {
            _ProductItemRepository = ProductItemRepository;
        }

        public async Task CreateAsync(ProductItem input)
        {
            await _ProductItemRepository.InsertAsync(input);
        }

        public async Task DeleteAsync(ProductItem input)
        {
            await _ProductItemRepository.DeleteAsync(input);
        }

        public  IQueryable<ProductItem> GetAll()
        {
            return  _ProductItemRepository.GetAll();
        }

        public async Task<ProductItem> GetAsync(int id)
        {
            try
            {
                var data = await _ProductItemRepository.GetAsync(id);
                return data;
            }
            catch (Exception )
            {

                throw;
            }
          
        }

        public async Task UpdateAsync(ProductItem input)
        {
             await _ProductItemRepository.UpdateAsync(input);
        }
    }
}
