using GTANetworkAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        #region Public Methods

        [ServerEvent(Event.PlayerDisconnected)]
        public async void PlayerDisconnected(IPlayer player, DisconnectionType disconnectionType, string reason)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is { })
            {
                await Init.TDSCore.EventsHandler.OnPlayerLoggedOut(tdsPlayer);
            }

            NAPI.Task.Run(() =>
            {
                Init.TDSCore.EventsHandler.OnPlayerDisconnected(player);
            });
        }

        #endregion Public Methods
    }
}
