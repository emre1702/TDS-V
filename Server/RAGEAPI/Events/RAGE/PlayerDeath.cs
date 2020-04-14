using GTANetworkAPI;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.PlayerDeath)]
        public void OnPlayerDeath(GTANetworkAPI.Player player, GTANetworkAPI.Player killer, uint reason)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            var tdsKiller = Init.GetTDSPlayerIfLoggedIn(killer) ?? tdsPlayer;

            Init.TDSCore.EventsHandler.OnPlayerDeath(tdsPlayer, tdsKiller, reason);
        }
    }
}
