using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS_Server.Data.Models.Map;

namespace TDS_Server.LobbySystem.MapHandlers
{
    public class RoundFightLobbyMapHandler : BaseLobbyMapHandler, IRoundFightLobbyMapHandler
    {
        public MapDto? CurrentMap { get; private set; }

        public RoundFightLobbyMapHandler(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events)
            : base(lobby, events)
        {
            events.RoundClear += RoundClear;
            events.RequestNewMap += GetNextMap;
            events.InitNewMap += Events_InitNewMap;
        }

        private void CreateMapLimitBlips(MapDto map)
        {
        }

        private void Events_InitNewMap(MapDto map)
        {
            CurrentMap = map;
            CreateMapLimitBlips(map);
        }

        private MapDto? GetNextMap()
        {
            return null;
        }

        private ValueTask RoundClear()
        {
            DeleteMapBlips();
            return default;
        }
    }
}
