using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.LobbySystem.Spectator
{
    public interface IRoundFightLobbySpectator : IFightLobbySpectator
    {
        Vector3 CurrentMapSpectatorPosition { get; }

        ValueTask SetPlayerCantBeSpectatedAnymore(ITDSPlayer player);
    }
}
