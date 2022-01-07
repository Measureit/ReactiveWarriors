using HotAndCold.Application.Messages;
using System;
using System.Threading.Tasks;

namespace HotAndCold.Application.MessageBus
{
    public interface IBusClient
    {
        Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage;
        Task<IDisposable> SubscribeAsync<TMessage>(Func<TMessage, Task> subscribeMethod, string @namespace = null) where TMessage : IMessage;
    }
}
