using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Data.Interfaces.PlayersSystem
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
