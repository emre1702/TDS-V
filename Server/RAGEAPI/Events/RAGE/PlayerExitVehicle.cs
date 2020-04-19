using GTANetworkAPI;
using TDS_Server.RAGEAPI.Player;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.PlayerExitVehicle)]
        public void PlayerExitVehicle(GTANetworkAPI.Player player, GTANetworkAPI.Vehicle vehicle)
        {
            var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            var modVehicle = Init.BaseAPI.EntityConvertingHandler.GetEntity(vehicle); 

            Init.TDSCore.EventsHandler.OnPlayerExitVehicle(tdsPlayer, modVehicle);
        }
    }
}
