using GTANetworkAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        #region Public Methods

        [ServerEvent(Event.PlayerSpawn)]
        public void OnPlayerDeath(IPlayer player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.EventsHandler.OnPlayerSpawn(tdsPlayer);
        }

        #endregion Public Methods
    }
}
