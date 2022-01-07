namespace HotAndCold.Application.Messages
{
    public interface IRejectedEvent : IEvent
    {
        string Reason { get; }
        string Code { get; }
    }
    public interface IRejectedEvent<TKey> : IEvent<TKey>, IRejectedEvent
    {

    }
}
