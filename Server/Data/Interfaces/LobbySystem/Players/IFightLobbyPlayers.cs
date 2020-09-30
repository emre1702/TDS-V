using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.LobbySystem.Players
{
    public interface IFightLobbyPlayers : IBaseLobbyPlayers
    {
        void Kill(ITDSPlayer player, string reason);
    }
}
