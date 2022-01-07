using HotAndCold.Application.MessageBus;
using HotAndCold.Contract;
using HotAndCold.Infrastructure.Notifications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace HotAndCold.HostedServices
{
    class NotificationHostedService : IHostedService
    {
        private readonly IHubContext<GameHub, IGameBroadcaster> _context;
        private readonly IPublisher _publisher;
        private readonly ISubscriber _subscriber;
        private readonly CompositeDisposable _subscriptions;
        public NotificationHostedService(
            IPublisher publisher,
            ISubscriber subscriber,
            IHubContext<GameHub, IGameBroadcaster> context)
        {
            _context = context;
            _publisher = publisher;
            _subscriber = subscriber;
            _subscriptions = new CompositeDisposable();
        }

        private Task HandleRoomCreated(RoomCreated @event)
            => _context.Clients.All.roomCreated(@event);

        public  Task HandleCreateRoom(CreateRoom command)
            => _publisher.PublishAsync(new RoomCreated(command.Id, command.RoomName));

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _subscriptions.Add(_subscriber.SubscribeEvent<RoomCreated>(HandleRoomCreated));
            _subscriptions.Add(_subscriber.SubscribeCommand<CreateRoom>(HandleCreateRoom));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _subscriptions.Dispose();
            return Task.CompletedTask;
        }
    }
}
