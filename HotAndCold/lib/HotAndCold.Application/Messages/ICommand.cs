using System;

namespace HotAndCold.Application.Messages
{
    public interface ICommand : IMessage { }
    public interface ICommand<TKey> : ICommand
    {
        public TKey Id { get; }
    }
}
