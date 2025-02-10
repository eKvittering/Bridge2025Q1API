using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Services;
using static Webminux.Optician.OpticianConsts;

public interface IInviteManager : IDomainService
{
    Task CreateAsync(Invite invite);
    Task CreateAsync(List<Invite> invites);
    Task UpdateInviteResponseAsync(int inviteId, InviteResponse inviteResponse);
    IQueryable<Invite> GetAll();
}