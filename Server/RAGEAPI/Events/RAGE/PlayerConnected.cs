using GTANetworkAPI;
using TDS_Server.RAGEAPI.Player;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.PlayerConnected)]
        public void PlayerConnected(GTANetworkAPI.Player player)
        {
            Init.BaseAPI.EntityConvertingHandler.PlayerConnected(player);

            var modPlayer = Init.BaseAPI.EntityConvertingHandler.GetEntity(player);
            if (modPlayer is null)
                return;

            Init.TDSCore.EventsHandler.OnPlayerConnected(modPlayer);
        }
    }
}
