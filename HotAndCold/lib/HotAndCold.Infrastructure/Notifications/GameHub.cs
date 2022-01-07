using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace HotAndCold.Infrastructure.Notifications
{
    public class GameHub : Hub<IGameBroadcaster>
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
