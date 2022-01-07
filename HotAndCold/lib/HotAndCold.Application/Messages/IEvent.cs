using System;

namespace HotAndCold.Application.Messages
{
    public interface IEvent : IMessage { }
    public interface IEvent<TKey> : IEvent
    {
        public TKey Id { get; }
    }
}
