using GTANetworkAPI;
using TDS_Server.RAGEAPI.Player;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.PlayerConnected)]
        public void PlayerConnected(GTANetworkAPI.Player player)
        {
            (Init.BaseAPI.Player as PlayerAPI)?.PlayerConnected(player);

            var tdsPlayer = Init.GetTDSPlayer(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.EventsHandler.OnPlayerConnected(tdsPlayer);
        }
    }
}
