using GTANetworkAPI;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        #region Public Methods

        [ServerEvent(Event.PlayerEnterVehicle)]
        public void PlayerEnterVehicle(GTANetworkAPI.Player player, GTANetworkAPI.Vehicle vehicle, sbyte seatId)
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

            Init.TDSCore.EventsHandler.OnPlayerEnterVehicle(tdsPlayer, tdsVehicle, seatId);*/
        }

        #endregion Public Methods
    }
}
