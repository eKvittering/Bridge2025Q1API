using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.PageConfigs.Dto;

namespace Webminux.Optician.PageConfigs
{
    internal interface IPageConfigAppService : IApplicationService
    {
        Task UpdateAsync(UpdatePageConfigDto input);

        Task<List<PageConfigDto>> GetAsync();
    }
}
