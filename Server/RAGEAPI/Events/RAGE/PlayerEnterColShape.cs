using GTANetworkAPI;
using TDS_Server.Data.Interfaces.ModAPI.ColShape;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.RAGEAPI.Player;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        #region Public Methods

        [ServerEvent(Event.PlayerEnterColshape)]
        public void OnPlayerEnterColshape(IColShape colShape, IPlayer player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            Init.TDSCore.EventsHandler.OnPlayerEnterColshape(colShape, tdsPlayer);
        }

        #endregion Public Methods
    }
}
