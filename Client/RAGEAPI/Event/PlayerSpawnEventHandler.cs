using System;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Handler;
using TDS_Shared.Data.Models;

namespace TDS_Client.RAGEAPI.Event
{
    internal class PlayerSpawnEventHandler : BaseEventHandler<SpawnDelegate>
    {
        #region Private Fields

        private readonly LoggingHandler _loggingHandler;

        #endregion Private Fields

        #region Public Constructors

        public PlayerSpawnEventHandler(LoggingHandler loggingHandler) : base()
        {
            _loggingHandler = loggingHandler;
            RAGE.Events.OnPlayerSpawn += PlayerSpawn;
        }

        #endregion Public Constructors

        #region Private Methods

        private void PlayerSpawn(RAGE.Events.CancelEventArgs cancelMod)
        {
            if (Actions.Count == 0)
                return;

            try
            {
                var cancel = new CancelEventArgs();

                for (int i = Actions.Count - 1; i >= 0; --i)
                {
                    var action = Actions[i];
                    if (action.Requirement is null || action.Requirement())
                        action.Method(cancel);
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
