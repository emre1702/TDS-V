using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Shared.Data.Models;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Sync
{
    public class ArenaSync : RoundFightLobbySync
    {
        public ArenaSync(IArena lobby, IRoundFightLobbyEventsHandler events)
            : base(lobby, events)
        {
        }

        protected override SyncedLobbySettings GetSyncedSettings(LobbyDb entity)
        {
            var settings = base.GetSyncedSettings(entity);

            settings.InLobbyWithMaps = true;

            return settings;
        }
    }
}
