using Abp.Domain.Repositories;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Webminux.Optician.Chat;
using Webminux.Optician.Hubs.Dto;

namespace Webminux.Optician.Hubs
{
    public class MessageHub : Hub
    {
        public async Task NewMessage(MessageDto msg)
        {
            await Clients.All.SendAsync("MessageReceived", msg);
        }
    }
}
