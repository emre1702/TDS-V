using GTANetworkAPI;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.PlayerSpawn)]
        public void OnPlayerDeath(GTANetworkAPI.Player player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.EventsHandler.OnPlayerSpawn(tdsPlayer);
        }
    }
}
