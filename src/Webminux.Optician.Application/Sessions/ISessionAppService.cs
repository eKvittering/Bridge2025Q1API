using System.Threading.Tasks;
using Abp.Application.Services;
using Webminux.Optician.Sessions.Dto;

namespace Webminux.Optician.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
