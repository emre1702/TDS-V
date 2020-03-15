using GTANetworkAPI;
using TDS_Server.RAGE.Player;
using TDS_Server.RAGE.Startup;
using TDS_Shared.Default;

namespace TDS_Server.RAGE.Events.Remote
{
    partial class BaseRemoteEvents : BaseEvents
    {
        [RemoteEvent(ToServerEvent.LobbyChatMessage)]
        public void LobbyChatMessage(GTANetworkAPI.Player player, string message, int chatTypeNumber)
        {
            var modPlayer = GetModPlayer(player);
            if (modPlayer is null)
                return;
            Program.TDSCore.EventsHandler.LobbyChatMessage(modPlayer, message, chatTypeNumber);
        }
    }
}
