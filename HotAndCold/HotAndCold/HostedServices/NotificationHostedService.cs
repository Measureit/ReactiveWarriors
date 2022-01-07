using HotAndCold.Infrastructure.Notifications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace HotAndCold.HostedServices
{
    class NotificationHostedService : IHostedService
    {
        private readonly IHubContext<GameHub, IGameBroadcaster> _context;

        public NotificationHostedService(IHubContext<GameHub, IGameBroadcaster> context)
        {
            _context = context;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _ = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    //await _context.Clients.All.gameAdded(new Game() { Date = DateTime.Now.ToString() });
                    await Task.Delay(1000);
                }
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}
