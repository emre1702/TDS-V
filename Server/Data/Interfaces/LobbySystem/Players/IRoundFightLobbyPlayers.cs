using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.LobbySystem.Players
{
    public interface IRoundFightLobbyPlayers : IFightLobbyPlayers
    {
        void SetPlayerDataAlive(ITDSPlayer player);

        void RespawnPlayer(ITDSPlayer player);
    }
}
