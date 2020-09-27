using GTANetworkAPI;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Models.Map;
using TDS_Server.LobbySystem.TeamHandlers;

namespace TDS_Server.LobbySystem.Spectator
{
    public class RoundFightLobbySpectator : FightLobbySpectator
    {
        private Vector3 _currentMapSpectatorPosition = new Vector3();

        public RoundFightLobbySpectator(RoundFightLobbyTeamsHandler teams, IRoundFightLobbyEventsHandler events) : base(teams)
        {
            events.InitNewMap += Events_InitNewMap;
        }

        private void Events_InitNewMap(MapDto map)
        {
            _currentMapSpectatorPosition = map.LimitInfo.Center?.ToVector3().AddToZ(10) ?? map.TeamSpawnsList.TeamSpawns[0].Spawns[0].ToVector3().AddToZ(10);
        }
    }
}
