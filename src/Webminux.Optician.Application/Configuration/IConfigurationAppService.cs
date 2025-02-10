using System.Threading.Tasks;
using Webminux.Optician.Configuration.Dto;

namespace Webminux.Optician.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
