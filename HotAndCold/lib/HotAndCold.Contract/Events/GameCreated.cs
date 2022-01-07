using System;

namespace HotAndCold.Contract.Commands
{
    public record GameCreated(Guid Id, string Name, string Code);
}
