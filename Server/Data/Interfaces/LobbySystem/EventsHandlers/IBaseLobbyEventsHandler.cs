using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity.Player;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers
{
#nullable enable

    public interface IBaseLobbyEventsHandler
    {
        public delegate void LobbyCreatedAfterDelegate(LobbyDb entity);

        public delegate void LobbyDelegate(IBaseLobby lobby);

        public delegate void PlayerDelegate(ITDSPlayer player);

        public delegate void PlayerJoinedDelegate(ITDSPlayer player, int teamIndex);

        public delegate void BanDelegate(PlayerBans ban);

        bool IsRemoved { get; }
        AsyncTaskEvent<LobbyDb>? Created { get; set; }
        AsyncTaskEvent<IBaseLobby>? Remove { get; set; }
        AsyncValueTaskEvent<ITDSPlayer>? PlayerLeft { get; set; }

        event LobbyCreatedAfterDelegate? CreatedAfter;

        event LobbyDelegate? RemoveAfter;

        event PlayerJoinedDelegate? PlayerJoined;

        event PlayerDelegate? PlayerLeftAfter;

        event BanDelegate? NewBan;

        Task TriggerCreated(LobbyDb entity);

        Task TriggerRemove();

        void TriggerPlayerJoined(ITDSPlayer player, int teamIndex);

        ValueTask TriggerPlayerLeft(ITDSPlayer player);

        void TriggerNewBan(PlayerBans ban, ulong? targetDiscordUserId);
    }
}
