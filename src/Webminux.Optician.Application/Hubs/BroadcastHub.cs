using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.ClientChat
{
    public interface IHubClient
    {
        Task BroadcastMessage();
    }
    public class BroadcastHub : Hub<IHubClient>
    {
    }
}
