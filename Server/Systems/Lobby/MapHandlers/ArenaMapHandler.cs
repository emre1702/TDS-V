using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Data.Models.Map;
using TDS.Server.Data.Models.Map.Creator;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Maps;
using TDS.Shared.Data.Default;
using TDS.Shared.Data.Enums;

namespace TDS.Server.LobbySystem.MapHandlers
{
    public class ArenaMapHandler : RoundFightLobbyMapHandler, IArenaMapHandler
    {
        protected new IArena Lobby => (IArena)base.Lobby;

        public ArenaMapHandler(IArena lobby, IRoundFightLobbyEventsHandler events, ISettingsHandler settingsHandler, MapsLoadingHandler mapsLoadingHandler)
            : base(lobby, events, settingsHandler, mapsLoadingHandler)
        {
        }

        protected override ValueTask Events_PlayerJoined((ITDSPlayer Player, int TeamIndex) data)
        {
            NAPI.Task.RunSafe(() =>
            {
                data.Player.Spawn(SpawnPoint.Around(Lobby.Entity.AroundSpawnPoint), SpawnRotation);
                data.Player.Freeze(true);
            });
            return default;
        }

        public Position4DDto? GetMapRandomSpawnData(ITeam? team)
        {
            if (CurrentMap is null)
                return null;
            if (team is null)
                return null;

            int index = team.SpawnCounter++;
            var teamSpawns = CurrentMap.TeamSpawnsList.TeamSpawns[team.Entity.Index - 1].Spawns;
            if (index >= teamSpawns.Count)
            {
                index = 0;
                team.SpawnCounter = 0;
            }
            return teamSpawns[index];
        }

        protected override void Events_InitNewMap(MapDto map)
        {
            base.Events_InitNewMap(map);
            CreateTeamSpawnBlips(map);
        }

        /// <summary>
        /// Creates a blip for every team-spawn "region". If many spawns are at one position/region,
        /// only one blip will get created. But if there are many spawn-regions, there will be
        /// multiple blips.
        /// </summary>
        /// <param name="map"></param>
        private void CreateTeamSpawnBlips(MapDto map)
        {
            Lobby.Teams.DoForList(teams =>
            {
                foreach (var teamsSpawnList in map.TeamSpawnsList.TeamSpawns)
                {
                    if (teams.Length < teamsSpawnList.TeamID)
                        return;
                    var regions = new List<Vector3>();
                    NAPI.Task.RunSafe(() =>
                    {
                        foreach (var spawns in teamsSpawnList.Spawns)
                        {
                            var position = spawns.ToVector3();
                            if (regions.Any(pos => pos.DistanceTo2D(position) < 5))
                                continue;
                            regions.Add(position);

                            var team = teams[(int)teamsSpawnList.TeamID];

                            var blip = NAPI.Blip.CreateBlip(SharedConstants.TeamSpawnBlipSprite, position, 1f, team.Entity.BlipColor,
                                name: "Spawn " + team.Entity.Name, dimension: Dimension) as ITDSBlip;

                            AddMapBlip(blip!);
                        }
                    });
                }
            });
        }

        public override MapDto? GetNextMap()
        {
            var map = Lobby.MapVoting.GetBoughtMap();
            if (map is { })
            {
                MapRetrieveType = MapRetrieveType.Bought;
                return map;
            }

            map = Lobby.MapVoting.GetVotedMap();
            if (map is { })
            {
                MapRetrieveType = MapRetrieveType.Voted;
                return map;
            }
            return base.GetNextMap();
        }
    }
}
