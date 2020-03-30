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

        [RemoteEvent(ToServerEvent.SaveSettings)]
        public void OnSaveSettings(GTANetworkAPI.Player player, string json)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteBrowserEventsHandler.OnSaveSettings(tdsPlayer, json);
        }

        [RemoteEvent(ToServerEvent.GetSupportRequestData)]
        public void OnGetSupportRequestData(GTANetworkAPI.Player player, int requestId)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteBrowserEventsHandler.OnGetSupportRequestData(tdsPlayer, requestId);
        }

        [RemoteEvent(ToServerEvent.SetSupportRequestClosed)]
        public void OnSetSupportRequestClosed(GTANetworkAPI.Player player, int requestId, bool closed)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteBrowserEventsHandler.OnSetSupportRequestClosed(tdsPlayer, requestId, closed);
        }

        [RemoteEvent(ToServerEvent.LeftSupportRequestsList)]
        public void OnLeftSupportRequestsList(GTANetworkAPI.Player player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteBrowserEventsHandler.OnLeftSupportRequestsList(tdsPlayer);
        }

        [RemoteEvent(ToServerEvent.LeftSupportRequest)]
        public void OnLeftSupportRequest(GTANetworkAPI.Player player, int requestId)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteBrowserEventsHandler.OnLeftSupportRequest(tdsPlayer, requestId);
        }

        [RemoteEvent(ToServerEvent.SendSupportRequest)]
        public void OnSendSupportRequest(GTANetworkAPI.Player player, string json)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteBrowserEventsHandler.OnSendSupportRequest(tdsPlayer, json);
        }

        [RemoteEvent(ToServerEvent.SendSupportRequestMessage)]
        public void OnSendSupportRequestMessage(GTANetworkAPI.Player player, int requestId, string message)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteBrowserEventsHandler.OnSendSupportRequestMessage(tdsPlayer, requestId, message);
        }
    }
}
