using GTANetworkAPI;
using TDS_Server.RAGE.Player;
using TDS_Server.RAGE.Startup;

namespace TDS_Server.RAGE.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.PlayerDisconnected)]
        public void PlayerDisconnected(GTANetworkAPI.Player player)
        {
            (Init.BaseAPI.Player as PlayerAPI)?.PlayerDisconnected(player);

            var modPlayer = Init.GetModPlayer(player);
            if (modPlayer is null)
                return;

            Init.TDSCore.EventsHandler.OnPlayerDisconnected(modPlayer);
        }
    }
}
