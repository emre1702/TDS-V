using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Models.Map;
using TDS_Shared.Default;

namespace TDS_Server.LobbySystem.Sync
{
    public class RoundFightLobbySync : BaseLobbySync
    {
        public RoundFightLobbySync(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events)
            : base(lobby, events)
        {
            events.InitNewMap += Events_InitNewMap;
            events.RoundClear += RoundClear;
        }

        private void Events_InitNewMap(MapDto map)
        {
            NAPI.Task.Run(() =>
                TriggerEvent(ToClientEvent.MapChange, map.ClientSyncedDataJson));
        }

        private ValueTask RoundClear()
        {
            NAPI.Task.Run(() =>
                TriggerEvent(ToClientEvent.MapClear));
            return default;
        }
    }
}
