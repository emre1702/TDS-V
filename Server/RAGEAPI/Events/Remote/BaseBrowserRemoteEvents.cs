using GTANetworkAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Shared.Default;

namespace TDS_Server.RAGEAPI.Events.Remote
{
    partial class BaseRemoteEvents
    {
        #region Public Methods

        [RemoteEvent(ToServerEvent.FromBrowserEvent)]
        public void OnFromBrowserEvent(IPlayer player, params object[] args)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.RemoteBrowserEventsHandler.OnFromBrowserEvent(tdsPlayer, args);
        }

        #endregion Public Methods
    }
}
