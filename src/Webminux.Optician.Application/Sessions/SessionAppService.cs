using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Auditing;
using Webminux.Optician.Companies;
using Webminux.Optician.Sessions.Dto;

namespace Webminux.Optician.Sessions
{
    /// <summary>
    /// Provide methods to store user,tenant and company information in session.
    /// </summary>
    public class SessionAppService : OpticianAppServiceBase, ISessionAppService
    {
        private readonly CompanyManager _companyManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionAppService"/> class.
        /// </summary>
        /// <param name="companyManager"></param>
        public SessionAppService(CompanyManager companyManager)
        {
            _companyManager = companyManager;
        }
        
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = System.DateTime.Now,
                    Features = new Dictionary<string, bool>()
                }
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
                output.Company= ObjectMapper.Map<CompanyLoginInfoDto>(await _companyManager.GetWithTenantIdAsync(AbpSession.TenantId.Value));
            }

            if (AbpSession.UserId.HasValue)
            {
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
            }

            return output;
        }
    }
}
