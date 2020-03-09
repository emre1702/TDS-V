using GTANetworkAPI;
using TDS_Server.RAGE.Events.Custom;

namespace TDS_Server.RAGE.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.PlayerDisconnected)]
        public void PlayerDisconnected(GTANetworkAPI.Player player)
        {
            BaseCustomEvents.PlayerDisconnectedInternal?.Invoke(player);
        }
    }
}
