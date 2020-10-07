using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
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

        public AsyncValueTaskEvent<(ITDSPlayer Player, int HadLifes)>? PlayerLeft { get; set; }

        public AsyncValueTaskEvent<(ITDSPlayer Player, int HadLifes)>? PlayerLeftAfter { get; set; }

        public AsyncValueTaskEvent<(ITDSPlayer Player, int TeamIndex)>? PlayerJoined { get; set; }
        public AsyncValueTaskEvent<(ITDSPlayer Player, int TeamIndex)>? PlayerJoinedAfter { get; set; }

        public event BanDelegate? NewBan;

        public event PlayerColshapeDelegate? PlayerEnteredColshape;

        public bool IsRemoved { get; private set; }

        private readonly EventsHandler _eventsHandler;
        private readonly IBaseLobby _lobby;
        protected readonly ILoggingHandler Logging;

        public BaseLobbyEventsHandler(IBaseLobby lobby, EventsHandler eventsHandler, ILoggingHandler logging)
            => (_lobby, _eventsHandler, Logging) = (lobby, eventsHandler, logging);

        public async Task TriggerCreated(LobbyDb entity)
        {
            var task = Created?.InvokeAsync(entity);
            if (task is { })
                await task.ConfigureAwait(false);
            CreatedAfter?.Invoke(entity);
        }

        public async Task TriggerRemove()
        {
            IsRemoved = true;
            var task = Remove?.InvokeAsync(_lobby);
            if (task is { })
                await task.ConfigureAwait(false);
            RemoveAfter?.Invoke(_lobby);
        }

        public async ValueTask TriggerPlayerLeft(ITDSPlayer player, int hadLifes)
        {
            var task = PlayerLeft?.InvokeAsync((player, hadLifes));
            if (task.HasValue)
                await task.Value.ConfigureAwait(false);
            task = PlayerLeftAfter?.InvokeAsync((player, hadLifes));
            if (task.HasValue)
                await task.Value.ConfigureAwait(false);
            _eventsHandler.OnLobbyLeaveNew(player, _lobby);
        }

        public async ValueTask TriggerPlayerJoined(ITDSPlayer player, int teamIndex)
        {
            var task = PlayerJoined?.InvokeAsync((player, teamIndex));
            if (task.HasValue)
                await task.Value.ConfigureAwait(false);
            task = PlayerJoinedAfter?.InvokeAsync((player, teamIndex));
            if (task.HasValue)
                await task.Value.ConfigureAwait(false);
            _eventsHandler.OnLobbyJoinedNew(player, _lobby);
        }

        public void TriggerNewBan(PlayerBans ban, ulong? targetDiscordUserId)
        {
            NewBan?.Invoke(ban);
            _eventsHandler.OnNewBan(ban, _lobby.Entity.IsOfficial, targetDiscordUserId);
        }

        public void TriggerPlayerEnteredColshape(ITDSColshape colshape, ITDSPlayer player)
        {
            PlayerEnteredColshape?.Invoke(colshape, player);
        }
    }
}
