using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.LobbySystem.Spectator
{
    public interface IRoundFightLobbySpectator : IFightLobbySpectator
    {
        Vector3 CurrentMapSpectatorPosition { get; }

        ValueTask SetPlayerCantBeSpectatedAnymore(ITDSPlayer player);
    }
}
