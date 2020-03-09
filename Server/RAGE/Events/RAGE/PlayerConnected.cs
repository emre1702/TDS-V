using GTANetworkAPI;
using TDS_Server.RAGE.Events.Custom;

namespace TDS_Server.RAGE.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.PlayerConnected)]
        public void PlayerConnected(GTANetworkAPI.Player player)
        {
            BaseCustomEvents.PlayerConnectedInternal?.Invoke(player);

            PedHash
        }
    }
}
