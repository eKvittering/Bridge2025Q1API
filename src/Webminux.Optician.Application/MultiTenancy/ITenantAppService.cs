using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Webminux.Optician.MultiTenancy.Dto;

namespace Webminux.Optician.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
        Task<PagedResultDto<TenantDto>> GetPagedTenantsAsync(PagedTenantResultRequestDto input);
        Task<TenantMediaDto> GetTenantMediaInfoAsync(string tenancyName);
        Task<TenantDto> GetTenantByIdAsync(int id);
    }
}

