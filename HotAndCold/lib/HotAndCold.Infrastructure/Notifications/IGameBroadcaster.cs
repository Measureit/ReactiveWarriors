using HotAndCold.Contract;
using System.Threading.Tasks;

namespace HotAndCold.Infrastructure.Notifications
{
    public interface IGameBroadcaster
    {
        Task roomCreated(RoomCreated @event);
    }
}
