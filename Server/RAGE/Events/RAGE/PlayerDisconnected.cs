using GTANetworkAPI;
using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Events.Mod;
using TDS_Server.RAGE.Startup;

namespace TDS_Server.RAGE.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.PlayerDisconnected)]
        public void PlayerDisconnected(GTANetworkAPI.Player player)
        {
            (Program.BaseAPI.Player as PlayerAPI)?.PlayerDisconnected(player);

            var modPlayer = (Program.BaseAPI.Player as PlayerAPI)?.GetIPlayer(player);
            if (modPlayer is null)
                return;
            Program.TDSCore.EventsHandler.OnPlayerDisconnected(modPlayer);
        }
    }
}
