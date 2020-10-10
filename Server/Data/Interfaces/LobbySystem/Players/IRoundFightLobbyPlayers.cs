using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.LobbySystem.Players
{
    public interface IRoundFightLobbyPlayers : IFightLobbyPlayers
    {
        bool SavePlayerLobbyStats { get; }

        void SetPlayerDataAlive(ITDSPlayer player);

        void RespawnPlayer(ITDSPlayer player);
    }
}
