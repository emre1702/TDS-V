using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Models.Map;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Default;

namespace TDS.Server.LobbySystem.Sync
{
    public class RoundFightLobbySync : BaseLobbySync
    {
        protected new IRoundFightLobbyEventsHandler Events => (IRoundFightLobbyEventsHandler)base.Events;

        public RoundFightLobbySync(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events)
            : base(lobby, events)
        {
            events.InitNewMap += Events_InitNewMap;
            events.RoundClear += RoundClear;
        }

        protected override void RemoveEvents(IBaseLobby lobby)
        {
            base.RemoveEvents(lobby);

            Events.InitNewMap -= Events_InitNewMap;
            if (Events.RoundClear is { })
                Events.RoundClear -= RoundClear;
        }

        private void Events_InitNewMap(MapDto map)
        {
            NAPI.Task.RunSafe(() =>
                TriggerEvent(ToClientEvent.MapChange, map.ClientSyncedDataJson));
        }

        private ValueTask RoundClear()
        {
            NAPI.Task.RunSafe(() =>
                TriggerEvent(ToClientEvent.MapClear));
            return default;
        }
    }
}