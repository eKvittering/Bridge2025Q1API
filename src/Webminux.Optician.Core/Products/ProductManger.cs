using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Products
{
    public class ProductManger : IProductManager
    {
        private readonly IRepository<Product> _productRepository;

        public ProductManger(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task CreateAsync(Product product)
        {
             await _productRepository.InsertAsync(product);
        }

        public async Task DeleteAsync(Product product)
        {
            await _productRepository.DeleteAsync(product);
        }

        public IQueryable<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Task<Product> GetAsync(int id)
        {
            return _productRepository.GetAsync(id);
        }

        public Task UpdateAsync(Product product)
        {
            return _productRepository.UpdateAsync(product);
        }


    }
}
