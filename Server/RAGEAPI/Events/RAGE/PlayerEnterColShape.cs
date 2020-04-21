using GTANetworkAPI;
using TDS_Server.RAGEAPI.Player;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.PlayerEnterColshape)]
        public void OnPlayerEnterColshape(GTANetworkAPI.ColShape colShape, GTANetworkAPI.Player player)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            var tdsColShape = new ColShape.ColShape(colShape);

            Init.TDSCore.EventsHandler.OnPlayerEnterColshape(tdsColShape, tdsPlayer);
        }
    }
}
