using GTANetworkAPI;
using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Handler;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.Core.Events
{
    public class ConnectionEvents : Script
    {
        [ServerEvent(Event.IncomingConnection)]
        public void IncomingConnection(string ip, string serial, string socialClubName, ulong socialClubId, GameTypes _, CancelEventArgs cancel)
        {
            EventsHandler.Instance.OnIncomingConnection(ip, serial, socialClubName, socialClubId, cancel);
        }

        [ServerEvent(Event.PlayerConnected)]
        public void PlayerConnected(ITDSPlayer player)
        {
            EventsHandler.Instance.OnPlayerConnected(player);
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public async void PlayerDisconnected(ITDSPlayer player, DisconnectionType _, string _2)
        {
            try
            {
                if (player.LoggedIn)
                    await EventsHandler.Instance.OnPlayerLoggedOut(player).ConfigureAwait(false);

                NAPI.Task.RunSafe(() =>
                {
                    EventsHandler.Instance.OnPlayerDisconnected(player);
                });
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }
    }
}
