using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Models.Map;

namespace TDS_Server.LobbySystem.MapHandlers
{
    public class ArenaMapHandler : BaseLobbyMapHandler
    {
        public ArenaMapHandler(IArena lobby, IRoundFightLobbyEventsHandler events) : base(lobby, events)
        {
            events.InitNewMap += CreateTeamSpawnBlips;
        }

        protected override void Events_PlayerJoined(ITDSPlayer player, int _)
        {
            NAPI.Task.Run(() =>
            {
                player.Spawn(SpawnPoint.Around(Lobby.Entity.AroundSpawnPoint), SpawnRotation);
                player.Freeze(true);
            });
        }

        private void CreateTeamSpawnBlips(MapDto map)
        {
        }
    }
}
