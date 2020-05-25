using GTANetworkAPI;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        #region Public Methods

        [ServerEvent(Event.PlayerExitVehicle)]
        public void PlayerExitVehicle(GTANetworkAPI.Player player, GTANetworkAPI.Vehicle vehicle)
        {
            /*var tdsPlayer = Init.GetTDSPlayerIfLoggedIn(player);
            if (tdsPlayer is null)
                return;

            var modVehicle = Init.BaseAPI.EntityConvertingHandler.GetEntity(vehicle);
            if (modVehicle is null)
                return;

            var tdsVehicle = Init.GetTDSVehicle(modVehicle);
            if (tdsVehicle is null)
                return;

            Init.TDSCore.EventsHandler.OnPlayerExitVehicle(tdsPlayer, tdsVehicle);*/
        }

        #endregion Public Methods
    }
}
