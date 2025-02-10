using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.UI;
using Webminux.Optician;

public class InviteManager : IInviteManager
{
    private readonly IRepository<Invite> _inviteRepository;
    public InviteManager(IRepository<Invite> InviteRepository)
    {
        _inviteRepository = InviteRepository;
    }
    public async Task CreateAsync(Invite invite)
    {
        await _inviteRepository.InsertAsync(invite);
    }

    public async Task CreateAsync(List<Invite> invites)
    {
        foreach (var invite in invites)
        {
            await _inviteRepository.InsertAsync(invite);
        }
    }

    public IQueryable<Invite> GetAll()
    {
        return _inviteRepository.GetAll();
    }

    public async Task UpdateInviteResponseAsync(int inviteId, OpticianConsts.InviteResponse inviteResponse)
    {
        var invite = await _inviteRepository.GetAsync(inviteId);
        ValidateInvite(invite);
        invite.Response = inviteResponse;
    }

    private static void ValidateInvite(Invite invite)
    {
        if (invite == null)
            throw new UserFriendlyException(OpticianConsts.ErrorMessages.InviteNotFound);
    }
}