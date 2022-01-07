using HotAndCold.Contract.Commands;
using System.Threading.Tasks;

namespace HotAndCold.Infrastructure.Notifications
{
    public interface IGameBroadcaster
    {
        Task gameCreated(GameCreated @event);
    }
}
