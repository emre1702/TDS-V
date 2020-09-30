using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.LobbySystem.Players
{
    public interface IRoundFightLobbyPlayers : IFightLobbyPlayers
    {
        void SetPlayerDataAlive(ITDSPlayer player);
    }
}
