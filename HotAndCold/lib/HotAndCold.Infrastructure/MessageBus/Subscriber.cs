using HotAndCold.Application.MessageBus;
using HotAndCold.Application.Messages;
using HotAndCold.Domain.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Threading.Tasks;

namespace HotAndCold.Infrastructure.MessageBus
{
    public class Subscriber : ISubscriber
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceProvider _appServiceProvider;
        private readonly IBusClient _busClient;
        private readonly int _retries;
        private readonly int _retryInterval;
        public Subscriber(IServiceProvider appServiceProvider)
        {
            _appServiceProvider = appServiceProvider;
            _logger = (ILogger<Subscriber>)_appServiceProvider.GetService(typeof(ILogger<Subscriber>));
            _serviceProvider = (IServiceProvider)_appServiceProvider.GetService(typeof(IServiceProvider));
            _busClient = (IBusClient)_serviceProvider.GetService(typeof(IBusClient));
            _retries = 0;
            _retryInterval = 2;
        }

        public IDisposable SubscribeCommand<TCommand>(Func<TCommand, Task> onReceive,
            Func<TCommand, DomainException, IRejectedEvent> onError = null)
            where TCommand : ICommand
        {
            return _busClient.SubscribeAsync<TCommand>(command =>
            {
                return TryHandleAsync(command,
                () =>
                {
                    return onReceive(command);
                }, onError);
            });
        }

        public IDisposable SubscribeEvent<TEvent>(
            Func<TEvent, Task> onReceive,
            Func<TEvent, DomainException, IRejectedEvent> onError = null)
            where TEvent : IEvent
        {
            return _busClient.SubscribeAsync<TEvent>(@event =>
            {
                return TryHandleAsync(@event,
                () =>
                {
                    return onReceive(@event);
                }, onError);
            });
        }

        private Task TryHandleAsync<TMessage>(TMessage message,
            Func<Task> handle, Func<TMessage, DomainException, IRejectedEvent> onError = null)
        {
            var currentRetry = 0;
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(_retries, i => TimeSpan.FromSeconds(_retryInterval));

            var messageName = message.GetType().Name;

            return retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    var retryMessage = currentRetry == 0
                          ? string.Empty
                          : $"Retry: {currentRetry}'.";
                    _logger.LogInformation($"Handling a message: '{messageName}'. {retryMessage}");

                    await handle();

                    _logger.LogInformation($"Handled a message: '{messageName}'. {retryMessage}");

                }
                catch (Exception exception)
                {
                    currentRetry++;
                    _logger.LogError(exception, exception.Message);
                    if (exception is DomainException demandException && onError != null)
                    {
                        var rejectedEvent = onError(message, demandException);
                        await _busClient.PublishAsync(rejectedEvent);
                        _logger.LogInformation($"Published a rejected event: '{rejectedEvent.GetType().Name}' " +
                                                 $"for the message: '{messageName}'.");

                    }

                    throw new Exception($"Unable to handle a message: '{messageName}' " +
                                          $"retry {currentRetry - 1}/{_retries}...");
                }
            });
        }
    }
}
