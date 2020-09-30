using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Models.Map;
using TDS_Server.Data.Models.Map.Creator;

namespace TDS_Server.LobbySystem.MapHandlers
{
    public class ArenaMapHandler : RoundFightLobbyMapHandler, IArenaMapHandler
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

        public Position4DDto? GetMapRandomSpawnData(ITeam? team)
        {
            if (CurrentMap is null)
                return null;
            if (team is null)
                return null;

            int index = team.SpawnCounter++;
            var teamSpawns = CurrentMap.TeamSpawnsList.TeamSpawns[team.Entity.Index - 1].Spawns;
            if (index >= teamSpawns.Length)
            {
                index = 0;
                team.SpawnCounter = 0;
            }
            return teamSpawns[index];
        }

        private void CreateTeamSpawnBlips(MapDto map)
        {
        }
    }
}
