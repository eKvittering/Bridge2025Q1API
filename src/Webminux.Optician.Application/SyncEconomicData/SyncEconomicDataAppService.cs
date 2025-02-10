using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Abp.UI;
using Hangfire;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.BackgroundJobs;
using Webminux.Optician.Brands;
using Webminux.Optician.Companies;
using Webminux.Optician.Helpers;
using Webminux.Optician.SyncEconomicData.Dto;

namespace Webminux.Optician.ImportEconomicData
{
    [AbpAuthorize]
    public class SyncEconomicDataAppService : OpticianAppServiceBase, ISyncEconomicDataAppService
    {
        private readonly IRepository<EconomicSyncHistory, int> _economicSyncHistoryRepository;
        private readonly IRepository<SyncHistoryDetail, int> _syncHistoryDetailRepository;
        private readonly UserManager _userManager;
        private readonly CompanyManager _companyManager;
        private readonly IObjectMapper _objectMapper;
        public SyncEconomicDataAppService(
            IRepository<EconomicSyncHistory, int> economicSyncHistoryRepository,
            IRepository<SyncHistoryDetail, int> syncHistoryDetailRepository,
            IObjectMapper objectMapper,
            UserManager userManager,
            CompanyManager companyManager)
        {
            _economicSyncHistoryRepository = economicSyncHistoryRepository;
            _syncHistoryDetailRepository = syncHistoryDetailRepository;
            _objectMapper = objectMapper;
            _userManager = userManager;
            _companyManager = companyManager;
        }

        public async Task InitializeSync()
        {
            var company = await _companyManager.GetWithTenantIdAsync(AbpSession.TenantId.Value);
            if (company == null)
                throw new UserFriendlyException("Company not found");
            try
            {
                var args = new DataImportJobInputDto
                {
                    SyncApiId = (int)company.SyncApiId,
                    UserId = AbpSession.UserId.Value,
                    TenantId = AbpSession.TenantId ?? 0
                };

                BackgroundJob.Enqueue<SyncDataFactory>(x => x.StartSync(args));
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException("Sync Failed");
            }
        }

        public List<EconomicSyncHistoryDto> GetSyncHistory()
        {
            var history = _economicSyncHistoryRepository.GetAll().OrderByDescending(x => x.Id).Take(5).Select(x => _objectMapper.Map<EconomicSyncHistoryDto>(x)).ToList();
            foreach (var item in history)
            {
                item.CustomersCount = _syncHistoryDetailRepository.Count(c => c.EconomicSyncHistoryId == item.Id && c.HistoryType == "Customer");
                item.ProductsCount = _syncHistoryDetailRepository.Count(c => c.EconomicSyncHistoryId == item.Id && c.HistoryType == "Product");
                item.ProductsGroupsCount = _syncHistoryDetailRepository.Count(c => c.EconomicSyncHistoryId == item.Id && c.HistoryType == "ProductGroup");
                item.InvoicesCount = _syncHistoryDetailRepository.Count(c => c.EconomicSyncHistoryId == item.Id && c.HistoryType == "Invoice");
            }
            return history;
        }

        #region  GetPagedResult
        /// <summary>
        /// Get Paged History
        /// </summary>
        public async Task<PagedResultDto<EconomicSyncHistoryDto>> GetPagedResultAsync(PagedResultRequestDto input)
        {
            var query = _economicSyncHistoryRepository.GetAll();
            query = ApplyFilters(input, query);
            IQueryable<EconomicSyncHistoryDto> selectQuery = GetSelectQuery(query);
            var result =  await selectQuery.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
            foreach (var item in result.Items)
            {
                item.CustomersCount = _syncHistoryDetailRepository.Count(c => c.EconomicSyncHistoryId == item.Id && c.HistoryType == "Customer");
                item.ProductsCount = _syncHistoryDetailRepository.Count(c => c.EconomicSyncHistoryId == item.Id && c.HistoryType == "Product");
                item.ProductsGroupsCount = _syncHistoryDetailRepository.Count(c => c.EconomicSyncHistoryId == item.Id && c.HistoryType == "ProductGroup");
                item.InvoicesCount = _syncHistoryDetailRepository.Count(c => c.EconomicSyncHistoryId == item.Id && c.HistoryType == "Invoice");
            }
            return result;
        }

        #region Private Methods
        private static IQueryable<EconomicSyncHistory> ApplyFilters(PagedResultRequestDto input, IQueryable<EconomicSyncHistory> query)
        {
            //if (string.IsNullOrWhiteSpace(input.Keyword) == false)
            //    query = query.Where(g => g.Name.Contains(input.Keyword));
            return query;
        }
        private IQueryable<EconomicSyncHistoryDto> GetSelectQuery(IQueryable<EconomicSyncHistory> query)
        {
            return query.Select(g => new EconomicSyncHistoryDto
            {
                Id = g.Id,
                IsInProcess = g.IsInProcess,
                CreatorUserId = g.CreatorUserId,
                CreationTime = g.CreationTime,
                IsFailed = g.IsFailed
            });
        }
        #endregion


        #endregion

        public async Task<List<SyncHistoryDetailDto>> GetSyncHistoryDetail(int economicHistoryId)
        {
            var detailList = new List<SyncHistoryDetailDto>();
            var list = await _syncHistoryDetailRepository.GetAllListAsync(x => x.EconomicSyncHistoryId == economicHistoryId);
            foreach (var item in list)
            {
                detailList.Add(new SyncHistoryDetailDto() { Id = item.Id, HistoryObjectTitle = item.HistoryObjectTitle, HistoryObjectId = item.HistoryObjectId, HistoryType = item.HistoryType, EconomicSyncHistoryId = item.EconomicSyncHistoryId });
            }
            return detailList;
        }

        public async Task<bool> IsUserHasEconomicGrants()
        {
            try
            {
                var company = await _companyManager.GetWithTenantIdAsync(AbpSession.TenantId.Value);
                if (company == null)
                    return false;
                return (!string.IsNullOrWhiteSpace(company.EconomicAppSecretToken)
                    && !string.IsNullOrWhiteSpace(company.EconomicAgreementGrantToken))
                    || (!string.IsNullOrWhiteSpace(company.BillyAccessToken));
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task SaveUserEconomicGrants(string appSecret, string userGrant)
        {
            var client = new RestClient("https://restapi.e-conomic.com/self");
            var request = new RestRequest();
            request.AddHeader("X-AppSecretToken", appSecret);
            request.AddHeader("X-AgreementGrantToken", userGrant);
            RestResponse response = await client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                var user = await base.GetCurrentUserAsync();
                // user.EconomicAgreementGrantToken = userGrant;
                // user.EconomicAppSecretToken = appSecret;
            }
            else
                throw new UserFriendlyException("Invalid Values provided");
        }

    }
}
