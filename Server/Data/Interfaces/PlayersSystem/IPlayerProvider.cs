using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerProvider
    {
        ITDSPlayer Create(NetHandle netHandle);
    }
}
