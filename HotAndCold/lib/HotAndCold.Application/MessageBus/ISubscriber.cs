using HotAndCold.Application.Messages;
using HotAndCold.Domain.Exceptions;
using System;
using System.Threading.Tasks;

namespace HotAndCold.Application.MessageBus
{
    public interface ISubscriber
    {
        IDisposable SubscribeCommand<TCommand>(
            Func<TCommand, Task> onReceive,
            Func<TCommand, DomainException, IRejectedEvent> onError = null)
            where TCommand : ICommand;

        IDisposable SubscribeEvent<TEvent>(
            Func<TEvent, Task> onReceive,
            Func<TEvent, DomainException, IRejectedEvent> onError = null)
                    where TEvent : IEvent;
    }
}
