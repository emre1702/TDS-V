using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Data.Interfaces.PlayersSystem
{
#nullable enable

    public interface IPlayerLobbyHandler
    {
        IBaseLobby? Current { get; }
        IBaseLobby? Previous { get; }
        PlayerLobbyStats? LobbyStats { get; }

        void Init(ITDSPlayer player);

        void SetLobby(IBaseLobby? lobby);

        Task SetPlayerLobbyStats(PlayerLobbyStats? playerLobbyStats);

        void SyncLobbyOwnerInfo();
    }
}
