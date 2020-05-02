using System;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Handler;
using TDS_Client.RAGEAPI.Entity;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;

namespace TDS_Client.RAGEAPI.Event
{
    class PlayerStartEnterVehicleEventHandler : BaseEventHandler<PlayerStartEnterVehicleDelegate>
    {
        private readonly LoggingHandler _loggingHandler;
        private readonly EntityConvertingHandler _entityConvertingHandler;

        public PlayerStartEnterVehicleEventHandler(LoggingHandler loggingHandler, EntityConvertingHandler entityConvertingHandler) 
            : base()
        {
            _loggingHandler = loggingHandler;
            _entityConvertingHandler = entityConvertingHandler;

            RAGE.Events.OnPlayerStartEnterVehicle += PlayerStartEnterVehicle;
        }

        private void PlayerStartEnterVehicle(RAGE.Elements.Vehicle modVehicle, int seatId, RAGE.Events.CancelEventArgs cancelMod)
        {
            if (Actions.Count == 0)
                return;

            try
            {
                var cancel = new CancelEventArgs();

                var vehicle = _entityConvertingHandler.GetEntity(modVehicle);

                for (int i = Actions.Count - 1; i >= 0; --i)
                {
                    var action = Actions[i];
                    if (action.Requirement is null || action.Requirement())
                        action.Method(vehicle, (VehicleSeat) seatId, cancel);
                }

                cancelMod.Cancel = cancel.Cancel;
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }
    }
}
