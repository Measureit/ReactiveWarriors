using HotAndCold.Application.MessageBus;
using HotAndCold.Contract;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace HotAndCold.Infrastructure.Notifications
{
    public class GameHub : Hub<IGameBroadcaster>
    {
        private readonly IPublisher _publisher;
        public GameHub(IPublisher publisher)
        {
            _publisher = publisher;
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task CreateRoom(string roomName)
        {
            var roomId = Guid.NewGuid();
            var command = new CreateRoom(roomId, roomName);
            await _publisher.SendAsync(command);
        }
    }
}
