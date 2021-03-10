using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Shared.Data.Models;
using LobbyDb = TDS.Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS.Server.LobbySystem.Sync
{
    public class GangActionLobbySync : RoundFightLobbySync
    {
        public GangActionLobbySync(IGangActionLobby lobby, IRoundFightLobbyEventsHandler events) : base(lobby, events)
        {
        }

        protected override SyncedLobbySettings CreateSyncedSettings(LobbyDb entity)
        {
            var settings = base.CreateSyncedSettings(entity);

            settings.CountdownTime = 0;
            settings.IsGangActionLobby = true;
            settings.InLobbyWithMaps = false;

            return settings;
        }
    }
}