using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.PageConfigs.Dto;

namespace Webminux.Optician.PageConfigs
{
    public class PageConfigAppService : OpticianAppServiceBase, IPageConfigAppService
    {
        private readonly IRepository<PageConfig, int> _repository;

        public PageConfigAppService(IRepository<PageConfig, int> repository)
        {
            _repository = repository;

        }
        public async Task<List<PageConfigDto>> GetAsync()
        {
            var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
            var pageConfigs = await getMenuItemDtoList();
            return pageConfigs.Where(s => s.TenantId == tenantId).ToList();
        }

        public async Task UpdateAsync(UpdatePageConfigDto input)
        {
            var data = await _repository.GetAsync(input.Id);
            if (data == null)
                throw new UserFriendlyException(OpticianConsts.ErrorMessages.PageConfigNotFound);

            data.Config = input.Config;
            await _repository.UpdateAsync(data);
        }

        private async Task<List<PageConfigDto>> getMenuItemDtoList()
        {
            return await (
                from cate in _repository.GetAll()
                select new PageConfigDto
                {
                    Id = cate.Id,
                    Name = cate.Name,
                    Config = cate.Config,
                    TenantId = cate.TenantId,
                    CreatorUserId = cate.CreatorUserId,
                    CreationTime = cate.CreationTime,
                }
            ).ToListAsync();
        }
    }
}
