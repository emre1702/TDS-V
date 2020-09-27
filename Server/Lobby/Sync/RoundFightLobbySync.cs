using System;
using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Players;
using TDS_Server.Data.Models.Map;
using TDS_Server.LobbySystem.TeamHandlers;
using TDS_Shared.Default;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Sync
{
    public class RoundFightLobbySync : BaseLobbySync
    {
        public RoundFightLobbySync(LobbyDb entity, IRoundFightLobbyEventsHandler events, Func<uint> dimensionProvider, IBaseLobbyPlayers players, BaseLobbyTeamsHandler teams)
            : base(entity, events, dimensionProvider, players, teams)
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
