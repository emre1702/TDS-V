using GTANetworkAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        #region Public Methods

        [ServerEvent(Event.PlayerDeath)]
        public void OnPlayerDeath(IPlayer player, IPlayer killer, uint reason)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            var tdsKiller = Init.GetTDSPlayerIfLoggedIn(killer) ?? tdsPlayer;

            Init.TDSCore.EventsHandler.OnPlayerDeath(tdsPlayer, tdsKiller, reason);
        }

        #endregion Public Methods
    }
}
