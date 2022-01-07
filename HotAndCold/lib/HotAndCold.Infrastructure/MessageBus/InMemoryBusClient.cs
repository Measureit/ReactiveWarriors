using HotAndCold.Application.MessageBus;
using HotAndCold.Application.Messages;
using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace HotAndCold.Infrastructure.MessageBus
{
    public class InMemoryBusClient : IBusClient
    {
        private readonly ISubject<IMessage> _bus;
        public InMemoryBusClient()
        {
            _bus = new Subject<IMessage>();
        }
        public Task PublishAsync<TMessage>(TMessage message)
            where TMessage : IMessage
        {
            _bus.OnNext(message);
            return Task.CompletedTask;
        }
        public Task<IDisposable> SubscribeAsync<TMessage>(Func<TMessage, Task> subscribeMethod, string @namespace = null) where TMessage : IMessage
        {
            var subscription = _bus
              .ObserveOn(Scheduler.Default)
              .Where(message => message.GetType().FullName.Equals(@namespace ?? typeof(TMessage).FullName))
                   .Select(x => Observable.FromAsync(async () => await ExecuteAsync<TMessage>(x, subscribeMethod)))
                   .Concat()
                   .Subscribe();
            return Task.FromResult(subscription);
        }

        async Task<Unit> ExecuteAsync<TMessage>(IMessage message, Func<TMessage, Task> subscribeMethod)
        {
            if (message is TMessage content)
            {
                await subscribeMethod(content);
            }
            return Unit.Default;
        }
    }
}
