using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Services;

namespace Webminux.Optician.Companies
{
    public interface ICompanyManager:IDomainService
    {
        Task CreateAsync(Company company);
        Task UpdateAsync(Company company);
        Task<Company> GetAsync(int id);
        Task<Company> GetFirstOrDefaultAsync(Expression<Func<Company, bool>> predicate);
        Task<Company> GetWithTenantIdAsync(int tenantId);
    }
}
