using System.Threading.Tasks;
using TDS_Server.Data.Utility;
using TDS_Server.LobbySystem.Lobbies;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.EventsHandlers
{
    public class BaseLobbyEventsHandler
    {
        public delegate void LobbyCreatedAfterDelegate(LobbyDb entity);

        public delegate void LobbyDelegate(BaseLobby lobby);

        public AsyncTaskEvent<LobbyDb>? LobbyCreated;

        public event LobbyCreatedAfterDelegate? LobbyCreatedAfter;

        public AsyncTaskEvent<BaseLobby>? LobbyRemove;

        public event LobbyDelegate? LobbyRemoveAfter;

        public async Task TriggerLobbyCreated(LobbyDb entity)
        {
            var task = LobbyCreated?.InvokeAsync(entity);
            if (task is { })
                await task;
            LobbyCreatedAfter?.Invoke(entity);
        }

        public async Task TriggerLobbyRemove(BaseLobby lobby)
        {
            var task = LobbyRemove?.InvokeAsync(lobby);
            if (task is { })
                await task;
            LobbyRemoveAfter?.Invoke(lobby);
        }
    }
}
