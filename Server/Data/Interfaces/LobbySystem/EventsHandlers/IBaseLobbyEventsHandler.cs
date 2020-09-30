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

        public delegate void BanDelegate(PlayerBans ban);

        bool IsRemoved { get; }
        AsyncTaskEvent<LobbyDb>? Created { get; set; }
        AsyncTaskEvent<IBaseLobby>? Remove { get; set; }
        AsyncValueTaskEvent<(ITDSPlayer player, int HadLifes)>? PlayerLeft { get; set; }

        event LobbyCreatedAfterDelegate? CreatedAfter;

        event LobbyDelegate? RemoveAfter;

        AsyncValueTaskEvent<(ITDSPlayer Player, int TeamIndex)>? PlayerJoined { get; set; }

        AsyncValueTaskEvent<(ITDSPlayer Player, int HadLifes)>? PlayerLeftAfter { get; set; }

        event BanDelegate? NewBan;

        Task TriggerCreated(LobbyDb entity);

        Task TriggerRemove();

        ValueTask TriggerPlayerJoined(ITDSPlayer player, int teamIndex);

        ValueTask TriggerPlayerLeft(ITDSPlayer player, int hadLifes);

        void TriggerNewBan(PlayerBans ban, ulong? targetDiscordUserId);
    }
}
