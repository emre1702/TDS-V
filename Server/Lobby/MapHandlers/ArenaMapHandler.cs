using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Models.Map;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.MapHandlers
{
    public class ArenaMapHandler : BaseLobbyMapHandler
    {
        public ArenaMapHandler(LobbyDb entity, IRoundFightLobbyEventsHandler events) : base(entity, events)
        {
            events.InitNewMap += CreateTeamSpawnBlips;
        }

        protected override void Events_PlayerJoined(ITDSPlayer player, int _)
        {
            NAPI.Task.Run(() =>
            {
                player.Spawn(SpawnPoint.Around(Entity.AroundSpawnPoint), SpawnRotation);
                player.Freeze(true);
            });
        }

        private void CreateTeamSpawnBlips(MapDto map)
        {
        }
    }
}
