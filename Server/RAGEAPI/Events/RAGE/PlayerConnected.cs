using GTANetworkAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        #region Public Methods

        [ServerEvent(Event.PlayerConnected)]
        public void PlayerConnected(IPlayer player)
        {
            Init.TDSCore.EventsHandler.OnPlayerConnected(player);
        }

        #endregion Public Methods
    }
}
