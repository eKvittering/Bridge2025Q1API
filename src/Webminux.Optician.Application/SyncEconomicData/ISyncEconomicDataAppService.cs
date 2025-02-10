using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Webminux.Optician.SyncEconomicData.Dto;

namespace Webminux.Optician.ImportEconomicData
{
    public interface ISyncEconomicDataAppService
    {
        Task InitializeSync();
        List<EconomicSyncHistoryDto> GetSyncHistory();
        Task<bool> IsUserHasEconomicGrants();
        Task SaveUserEconomicGrants(string appSecret, string userGrant);
        Task<List<SyncHistoryDetailDto>> GetSyncHistoryDetail(int economicHistoryId);

        Task<PagedResultDto<EconomicSyncHistoryDto>> GetPagedResultAsync(PagedResultRequestDto input);
    }
}
