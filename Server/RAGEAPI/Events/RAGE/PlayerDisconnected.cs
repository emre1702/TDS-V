using GTANetworkAPI;
using TDS_Server.RAGEAPI.Player;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.PlayerDisconnected)]
        public async void PlayerDisconnected(GTANetworkAPI.Player player, DisconnectionType disconnectionType, string reason)
        {
            (Init.BaseAPI.Player as PlayerAPI)?.PlayerDisconnected(player);

            var modPlayer = Init.GetModPlayer(player);
            if (modPlayer is null)
                return;

            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is { })
            {
                await Init.TDSCore.EventsHandler.OnPlayerLoggedOut(tdsPlayer);
            }

            Init.TDSCore.EventsHandler.OnPlayerDisconnected(modPlayer);
        }
    }
}
