using HotAndCold.Application.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace HotAndCold.Infrastructure.MessageBus
{
    public static class Extensions
    {
        public static void AddInMemoryMessageBus(this IServiceCollection services)
        {
            services
                .AddSingleton<IBusClient, InMemoryBusClient>()
                .AddTransient<ISubscriber, Subscriber>()
                .AddTransient<IPublisher, Publisher>();
        }
    }
}
