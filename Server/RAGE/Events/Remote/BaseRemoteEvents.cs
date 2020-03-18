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
            var tdsPlayer = Program.GetTDSPlayer(player);
            if (tdsPlayer is null)
                return;

            Program.TDSCore.RemoteEventsHandler.LobbyChatMessage(tdsPlayer, message, chatTypeNumber);
        }

        [RemoteEvent(ToServerEvent.TryLogin)]
        public void TryLogin(GTANetworkAPI.Player player, string username, string password)
        {
            var tdsPlayer = Program.GetTDSPlayer(player);
            if (tdsPlayer is null)
                return;

            Program.TDSCore.RemoteEventsHandler.TryLogin(tdsPlayer, username, password);
        }

        [RemoteEvent(ToServerEvent.ToggleMapFavouriteState)]
        public void ToggleMapFavouriteState(GTANetworkAPI.Player player, int mapId, bool isFavorite)
        {
            var tdsPlayer = Program.GetTDSPlayer(player);
            if (tdsPlayer is null)
                return;

            Program.TDSCore.RemoteEventsHandler.ToggleMapFavouriteState(tdsPlayer, mapId, isFavorite);
        }
    }
}
