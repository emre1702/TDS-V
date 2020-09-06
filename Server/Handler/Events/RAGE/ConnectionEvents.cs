using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Handler.Events.RAGE
{
    public class ConnectionEvents : Script
    {
        [ServerEvent(Event.IncomingConnection)]
        public void IncomingConnection(string ip, string serial, string socialClubName, ulong socialClubId, GameTypes gameType, CancelEventArgs cancel)
        {
            EventsHandler.Instance.OnIncomingConnection(ip, serial, socialClubName, socialClubId, cancel);
        }

        [ServerEvent(Event.PlayerConnected)]
        public void PlayerConnected(ITDSPlayer player)
        {
            EventsHandler.Instance.OnPlayerConnected(player);
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public async void PlayerDisconnected(ITDSPlayer player, DisconnectionType disconnectionType, string reason)
        {
            if (player.LoggedIn)
                await EventsHandler.Instance.OnPlayerLoggedOut(player);

            NAPI.Task.Run(() =>
            {
                EventsHandler.Instance.OnPlayerDisconnected(player);
            });
        }
    }
}
