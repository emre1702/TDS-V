using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Shared.Data.Models;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Sync
{
    public class GangActionLobbySync : RoundFightLobbySync
    {
        public GangActionLobbySync(IGangActionLobby lobby, IRoundFightLobbyEventsHandler events) : base(lobby, events)
        {
        }

        protected override SyncedLobbySettings GetSyncedSettings(LobbyDb entity)
        {
            var settings = base.GetSyncedSettings(entity);

            settings.CountdownTime = 0;
            settings.IsGangActionLobby = true;
            settings.InLobbyWithMaps = false;

            return settings;
        }
    }
}
