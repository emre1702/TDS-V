using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Sync
{
    public class ArenaLobbySync : BaseLobbySync
    {
        public ArenaLobbySync(LobbyDb entity) : base(entity)
        {
        }

        protected override void InitSyncedSettings(LobbyDb entity)
        {
            base.InitSyncedSettings(entity);

            SyncedSettings.InLobbyWithMaps = true;
        }
    }
}
