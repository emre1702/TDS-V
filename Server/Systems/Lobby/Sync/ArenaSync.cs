using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Shared.Data.Models;
using LobbyDb = TDS.Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS.Server.LobbySystem.Sync
{
    public class ArenaSync : RoundFightLobbySync
    {
        public ArenaSync(IArena lobby, IRoundFightLobbyEventsHandler events)
            : base(lobby, events)
        {
        }

        protected override SyncedLobbySettings CreateSyncedSettings(LobbyDb entity)
        {
            var settings = base.CreateSyncedSettings(entity);

            settings.InLobbyWithMaps = true;

            return settings;
        }
    }
}
