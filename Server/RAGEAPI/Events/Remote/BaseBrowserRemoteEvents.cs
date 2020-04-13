using GTANetworkAPI;
using TDS_Shared.Default;

namespace TDS_Server.RAGEAPI.Events.Remote
{
    partial class BaseRemoteEvents
    {
        [RemoteEvent(ToServerEvent.FromBrowserEvent)]
        public void OnFromBrowserEvent(GTANetworkAPI.Player player, params object[] args)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteBrowserEventsHandler.OnFromBrowserEvent(tdsPlayer, args);
        }
    }
}
