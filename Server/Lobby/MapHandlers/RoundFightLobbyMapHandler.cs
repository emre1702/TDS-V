using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.MapHandlers;
using TDS_Server.Data.Models.Map;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.MapHandlers
{
    public class RoundFightLobbyMapHandler : BaseLobbyMapHandler, IRoundFightLobbyMapHandler
    {
        public MapDto? CurrentMap { get; private set; }

        public RoundFightLobbyMapHandler(LobbyDb entity, IRoundFightLobbyEventsHandler events)
            : base(entity, events)
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
