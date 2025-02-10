using System.Threading.Tasks;
using Abp.Application.Services;
using Webminux.Optician.Authorization.Accounts.Dto;

namespace Webminux.Optician.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
