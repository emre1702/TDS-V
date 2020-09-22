using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.MapHandlers
{
    public class MainMenuMapHandler : BaseLobbyMapHandler
    {
        public MainMenuMapHandler(LobbyDb entity, IBaseLobbyEventsHandler events) : base(entity, events)
        {
        }

        protected override void Events_PlayerJoined(ITDSPlayer player)
        {
            NAPI.Task.Run(() =>
            {
                player.Spawn(SpawnPoint.Around(Entity.AroundSpawnPoint), SpawnRotation);
                player.Freeze(true);
            });
        }
    }
}
