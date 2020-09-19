using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Utility;
using TDS_Server.Handler.Events;
using static TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers.IBaseLobbyEventsHandler;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.EventsHandlers
{
    public class BaseLobbyEventsHandler : IBaseLobbyEventsHandler
    {
        public AsyncTaskEvent<LobbyDb>? LobbyCreated { get; set; }

        public event LobbyCreatedAfterDelegate? LobbyCreatedAfter;

        public AsyncTaskEvent<IBaseLobby>? LobbyRemove { get; set; }

        public event LobbyDelegate? LobbyRemoveAfter;

        public AsyncValueTaskEvent<ITDSPlayer>? PlayerLeftLobby { get; set; }

        public event PlayerDelegate? PlayerLeftLobbyAfter;

        public event PlayerDelegate? PlayerJoinedLobby;

        public bool IsRemoved { get; private set; }

        private readonly EventsHandler _eventsHandler;
        private readonly IBaseLobby _lobby;

        public BaseLobbyEventsHandler(EventsHandler eventsHandler, IBaseLobby lobby)
            => (_eventsHandler, _lobby) = (eventsHandler, lobby);

        public async Task TriggerLobbyCreated(LobbyDb entity)
        {
            var task = LobbyCreated?.InvokeAsync(entity);
            if (task is { })
                await task;
            LobbyCreatedAfter?.Invoke(entity);
        }

        public async Task TriggerLobbyRemove(IBaseLobby lobby)
        {
            IsRemoved = true;
            var task = LobbyRemove?.InvokeAsync(lobby);
            if (task is { })
                await task;
            LobbyRemoveAfter?.Invoke(lobby);
        }

        public async ValueTask TriggerPlayerLeftLobby(ITDSPlayer player)
        {
            var task = PlayerLeftLobby?.InvokeAsync(player);
            if (task.HasValue)
                await task.Value;
            PlayerLeftLobbyAfter?.Invoke(player);
            _eventsHandler.OnLobbyLeaveNew(player, _lobby);
        }

        public void TriggerPlayerJoinedLobby(ITDSPlayer player)
        {
            PlayerJoinedLobby?.Invoke(player);
        }
    }
}
