using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Helpers
{
    public static class IQueryableExtentions
    {
        public static async Task<PagedResultDto<T>> GetPagedResultAsync<T>(this IQueryable<T> query, int skipCount, int maxResultCount) where T : ICreationAudited
        {
            var totalCount = query.Count();
            var result = await query.OrderByDescending(q => q.CreationTime).Skip(skipCount).Take(maxResultCount).ToListAsync();
            return new PagedResultDto<T>(totalCount, result);
        }

        public static async Task<ListResultDto<LookUpDto<TKey>>> GetLookupResultAsync<T, TKey>(this IQueryable<T> query) where T : ILookupDto<TKey>
        {
            IQueryable<LookUpDto<TKey>> selectQuery = GetLookupSelectQuery<T, TKey>(query);
            var result = await selectQuery.ToListAsync();
            return new ListResultDto<LookUpDto<TKey>>(result);
        }

        #region Private Methods
        private static IQueryable<LookUpDto<TKey>> GetLookupSelectQuery<T, TKey>(IQueryable<T> query) where T : ILookupDto<TKey>
        {
            return query.Select(q => new LookUpDto<TKey>
            {
                Id = q.Id,
                Name = q.Name
            });
        }
        #endregion
    }
}
