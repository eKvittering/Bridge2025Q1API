using Abp.Dependency;
using System.Threading.Tasks;

namespace Webminux.Optician.BackgroundJobs
{
    public class SyncDataFactory : ITransientDependency
    {

        private readonly SyncEconomicDataJob _syncEconomicData;
        private readonly SyncBillyDataJob _syncBillyData;

        public SyncDataFactory(SyncEconomicDataJob syncEconomicData, SyncBillyDataJob syncBillyData)
        {
            _syncEconomicData = syncEconomicData;
            _syncBillyData = syncBillyData;
        }


        private SyncDataJobBase GetSyncRepo(int SyncApiId)
        {
            switch (SyncApiId)
            {
                case (int)OpticianConsts.SyncApis.Economic:
                    return _syncEconomicData;
                case (int)OpticianConsts.SyncApis.Billy:
                    return _syncBillyData;
                default:
                    return null;
            }
        }
        public async Task StartSync(DataImportJobInputDto data)
        {
            var syncDataJob = GetSyncRepo(data.SyncApiId);
            if (syncDataJob != null)
                await syncDataJob.Execute(data);
        }
    }
}
