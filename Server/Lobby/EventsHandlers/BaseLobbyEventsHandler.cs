using System.Threading.Tasks;
using TDS_Server.Data.Utility;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.EventsHandlers
{
    public class BaseLobbyEventsHandler
    {
        public delegate void LobbyCreatedAfterDelegate(LobbyDb entity);

        public AsyncTaskEvent<LobbyDb>? LobbyCreated;

        public event LobbyCreatedAfterDelegate? LobbyCreatedAfter;

        public async Task TriggerLobbyCreated(LobbyDb entity)
        {
            var task = LobbyCreated?.InvokeAsync(entity);
            if (task is { })
                await task;
            LobbyCreatedAfter?.Invoke(entity);
        }
    }
}
