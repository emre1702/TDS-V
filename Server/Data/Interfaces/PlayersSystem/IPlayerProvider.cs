using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerProvider
    {
        ITDSPlayer Create(NetHandle netHandle);
    }
}
