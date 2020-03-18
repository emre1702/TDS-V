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
            (Program.BaseAPI.Player as PlayerAPI)?.PlayerDisconnected(player);

            var modPlayer = Program.GetModPlayer(player);
            if (modPlayer is null)
                return;

            Program.TDSCore.EventsHandler.OnPlayerDisconnected(modPlayer);
        }
    }
}
