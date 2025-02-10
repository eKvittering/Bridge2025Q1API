using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Orders
{
    public interface IOrderManager : IDomainService
    {
        Task CreateAsync(Order order);
        Task UpdateAsync(Order order);
        IQueryable<Order> GetAll();
    }
}
