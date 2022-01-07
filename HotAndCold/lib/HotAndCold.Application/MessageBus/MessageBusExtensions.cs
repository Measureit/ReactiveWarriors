﻿using HotAndCold.Application.Messages;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HotAndCold.Application.MessageBus
{
    public static class MessageBusExtensions
    {
        public static ISubscriber SubscribeAllMessages(this ISubscriber subscriber, IEnumerable<Assembly> messagesAssembly)
            => subscriber
                .SubscribeAllCommands(messagesAssembly)
                .SubscribeAllEvents(messagesAssembly);

        public static ISubscriber SubscribeAllCommands(this ISubscriber subscriber, IEnumerable<Assembly> messagesAssembly)
            => subscriber
                .SubscribeAllMessages<ICommand>(messagesAssembly, nameof(ISubscriber.SubscribeCommand));

        public static ISubscriber SubscribeAllEvents(this ISubscriber subscriber, IEnumerable<Assembly> messagesAssembly)
            => subscriber
                .SubscribeAllMessages<IEvent>(messagesAssembly, nameof(ISubscriber.SubscribeEvent));

        private static ISubscriber SubscribeAllMessages<TMessage>
            (this ISubscriber subscriber, IEnumerable<Assembly> messagesAssembly, string subscribeMethod)
        {
            var messageTypes = messagesAssembly.SelectMany(assembly =>
                assembly.GetTypes()
                .Where(t => t.IsClass && typeof(TMessage).IsAssignableFrom(t))
                .Where(t => !t.IsAbstract))
                .ToList();

            messageTypes.ForEach(mt => subscriber.GetType()
                .GetMethod(subscribeMethod)
                .MakeGenericMethod(mt)
                .Invoke(subscriber,
                    new object[] { null, null, null }));

            return subscriber;
        }
    }
}

