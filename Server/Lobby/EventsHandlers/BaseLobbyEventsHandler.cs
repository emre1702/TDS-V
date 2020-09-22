using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Events;
using static TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers.IBaseLobbyEventsHandler;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.EventsHandlers
{
    public class BaseLobbyEventsHandler : IBaseLobbyEventsHandler
    {
        public AsyncTaskEvent<LobbyDb>? Created { get; set; }

        public event LobbyCreatedAfterDelegate? CreatedAfter;

        public AsyncTaskEvent<IBaseLobby>? Remove { get; set; }

        public event LobbyDelegate? RemoveAfter;

        public AsyncValueTaskEvent<ITDSPlayer>? PlayerLeft { get; set; }

        public event PlayerDelegate? PlayerLeftAfter;

        public event PlayerJoinedDelegate? PlayerJoined;

        public event BanDelegate? NewBan;

        public bool IsRemoved { get; private set; }

        private readonly EventsHandler _eventsHandler;
        private readonly IBaseLobby _lobby;

        public BaseLobbyEventsHandler(EventsHandler eventsHandler, IBaseLobby lobby)
            => (_eventsHandler, _lobby) = (eventsHandler, lobby);

        public async Task TriggerCreated(LobbyDb entity)
        {
            var task = Created?.InvokeAsync(entity);
            if (task is { })
                await task;
            CreatedAfter?.Invoke(entity);
        }

        public async Task TriggerRemove(IBaseLobby lobby)
        {
            IsRemoved = true;
            var task = Remove?.InvokeAsync(lobby);
            if (task is { })
                await task;
            RemoveAfter?.Invoke(lobby);
        }

        public async ValueTask TriggerPlayerLeft(ITDSPlayer player)
        {
            var task = PlayerLeft?.InvokeAsync(player);
            if (task.HasValue)
                await task.Value;
            PlayerLeftAfter?.Invoke(player);
            _eventsHandler.OnLobbyLeaveNew(player, _lobby);
        }

        public void TriggerPlayerJoined(ITDSPlayer player, int teamIndex)
        {
            PlayerJoined?.Invoke(player, teamIndex);
            _eventsHandler.OnLobbyJoinedNew(player, _lobby);
        }

        public void TriggerNewBan(PlayerBans ban, ulong? targetDiscordUserId)
        {
            NewBan?.Invoke(ban);
            _eventsHandler.OnNewBan(ban, _lobby.Entity.IsOfficial, targetDiscordUserId);
        }
    }
}
