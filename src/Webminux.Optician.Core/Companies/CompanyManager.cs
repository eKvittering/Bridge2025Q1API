using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;

namespace Webminux.Optician.Companies
{
    public class CompanyManager : ICompanyManager
    {
        private readonly IRepository<Company, int> _repository;

        public CompanyManager(IRepository<Company, int> repository)
        {
            _repository = repository;
        }
        public async Task CreateAsync(Company company)
        {
            await _repository.InsertAsync(company);
        }

        public async Task<Company> GetAsync(int id)
        {
            var company = await _repository.GetAsync(id);
            return company;
        }

        public async Task UpdateAsync(Company company)
        {
            await _repository.UpdateAsync(company);
        }

        public async Task<Company> GetFirstOrDefaultAsync(Expression<Func<Company, bool>> predicate)
        {
            var company = await _repository.FirstOrDefaultAsync(predicate);
            return company;
        }

        public async Task<Company> GetWithTenantIdAsync(int tenantId)
        {
            var company = await _repository.FirstOrDefaultAsync(company => company.TenantId == tenantId);
            return company;
        }
    }
}
