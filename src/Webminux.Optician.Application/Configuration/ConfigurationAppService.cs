using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Webminux.Optician.Configuration.Dto;

namespace Webminux.Optician.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : OpticianAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
