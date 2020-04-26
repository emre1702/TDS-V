using GTANetworkAPI;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.PlayerDisconnected)]
        public async void PlayerDisconnected(GTANetworkAPI.Player player, DisconnectionType disconnectionType, string reason)
        {
            var modPlayer = Init.GetModPlayer(player);
            if (modPlayer is null)
                return;

            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is { })
            {
                await Init.TDSCore.EventsHandler.OnPlayerLoggedOut(tdsPlayer);
            }

            NAPI.Task.Run(() =>
            {
                Init.TDSCore.EventsHandler.OnPlayerDisconnected(modPlayer);

                Init.BaseAPI.EntityConvertingHandler.PlayerDisconnected(player);
            });
            
        }
    }
}
