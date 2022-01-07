using HotAndCold.Application.Messages;
using System;

namespace HotAndCold.Contract
{
    public record CreateRoom(Guid Id, string RoomName) : ICommand<Guid>;
}
