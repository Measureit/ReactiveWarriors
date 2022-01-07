using HotAndCold.Application.MessageBus;
using HotAndCold.Application.Messages;
using System.Threading.Tasks;

namespace HotAndCold.Infrastructure.MessageBus
{
    class Publisher : IPublisher
    {
        private readonly IBusClient _busClient;

        public Publisher(IBusClient busClient)
        {
            _busClient = busClient;
        }

        public Task SendAsync<TCommand>(TCommand command)
          where TCommand : ICommand
          => _busClient.PublishAsync(command);

        public Task PublishAsync<TEvent>(TEvent @event)
          where TEvent : IEvent
          => _busClient.PublishAsync(@event);
    }
}
