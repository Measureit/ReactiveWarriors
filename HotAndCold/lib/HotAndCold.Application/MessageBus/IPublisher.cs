using HotAndCold.Application.Messages;
using System.Threading.Tasks;

namespace HotAndCold.Application.MessageBus
{
    public interface IPublisher
    {
        Task SendAsync<TCommand>(TCommand command)
            where TCommand : ICommand;

        Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : IEvent;
    }
}
