using HotAndCold.Application.Messages;
using System;

namespace HotAndCold.Contract
{
    public record RoomCreated(Guid Id, string RoomName) : IEvent<Guid>;
}
