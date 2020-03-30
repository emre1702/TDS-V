using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Sync;

namespace TDS_Server.RAGEAPI.Sync
{
    class SyncAPI : ISyncAPI
    {

        public void SendEvent(ITDSPlayer player, string eventName, params object[] args)
        {
            if (!(player.ModPlayer is Player.Player modPlayer))
                return;
            NAPI.ClientEvent.TriggerClientEvent(modPlayer._instance, eventName, args);
        }

        public void SendEvent(string eventName, params object[] args)
        {
            NAPI.ClientEvent.TriggerClientEventForAll(eventName, args);
        }

        public void SendEvent(ILobby lobby, string eventName, params object[] args)
        {
            NAPI.ClientEvent.TriggerClientEventInDimension(lobby.Dimension, eventName, args);
        }

        public void SendEvent(ICollection<ITDSPlayer> players, string eventName, params object[] args)
        {
            NAPI.ClientEvent.TriggerClientEventToPlayers(players.Select(p => p.ModPlayer).OfType<Player.Player>().Select(p => p._instance).ToArray(), eventName, args);
        }

        public void SendEvent(ITeam team, string eventName, params object[] args)
        {
            SendEvent(team.Players, eventName, args);
        }
    }
}
