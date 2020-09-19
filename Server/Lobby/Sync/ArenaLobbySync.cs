using System;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Sync
{
    public class ArenaLobbySync : BaseLobbySync
    {
        public ArenaLobbySync(LobbyDb entity, IBaseLobbyEventsHandler events, Func<uint> dimensionProvider) : base(entity, events, dimensionProvider)
        {
        }

        protected override void InitSyncedSettings(LobbyDb entity)
        {
            base.InitSyncedSettings(entity);

            SyncedSettings.InLobbyWithMaps = true;
        }
    }
}
