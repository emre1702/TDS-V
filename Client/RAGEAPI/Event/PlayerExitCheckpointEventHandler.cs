﻿using System;
using TDS_Client.Data.Interfaces.RAGE.Game.Checkpoint;
using TDS_Client.Data.Interfaces.RAGE.Game.Event;
using TDS_Client.Data.Interfaces.RAGE.Game.Vehicle;
using TDS_Client.Handler;
using TDS_Shared.Data.Models;

namespace TDS_Client.RAGEAPI.Event
{
    internal class PlayerExitCheckpointEventHandler : BaseEventHandler<PlayerExitCheckpointDelegate>
    {
        #region Private Fields

        private readonly LoggingHandler _loggingHandler;

        #endregion Private Fields

        #region Public Constructors

        public PlayerExitCheckpointEventHandler(LoggingHandler loggingHandler)
            : base()
        {
            _loggingHandler = loggingHandler;

            RAGE.Events.OnPlayerExitCheckpoint += OnPlayerExitCheckpoint;
        }

        #endregion Public Constructors

        #region Private Methods

        private void OnPlayerExitCheckpoint(RAGE.Elements.Checkpoint checkPoint, RAGE.Events.CancelEventArgs cancelMod)
        {
            if (Actions.Count == 0)
                return;

            try
            {
                var cancel = new CancelEventArgs();

                var vehicle = checkPoint as IVehicle;

                for (int i = Actions.Count - 1; i >= 0; --i)
                {
                    var action = Actions[i];
                    if (action.Requirement is null || action.Requirement())
                        action.Method(checkPoint as ICheckpoint, cancel);
                }

                cancelMod.Cancel = cancel.Cancel;
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        #endregion Private Methods
    }
}