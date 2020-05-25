using System;
using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Handler;
using TDS_Shared.Core;

namespace TDS_Client.RAGEAPI.Event
{
    public class TickEventHandler : BaseEventHandler<TickDelegate>
    {
        #region Private Fields

        private readonly LoggingHandler _loggingHandler;

        #endregion Private Fields

        #region Public Constructors

        public TickEventHandler(LoggingHandler loggingHandler)
        {
            _loggingHandler = loggingHandler;
            RAGE.Events.Tick += OnTick;
        }

        #endregion Public Constructors

        #region Private Methods

        private void OnTick(List<RAGE.Events.TickNametagData> nametags)
        {
            try
            {
                int currentMs = TDSTimer.ElapsedMs;
                for (int i = Actions.Count - 1; i >= 0; --i)
                {
                    var action = Actions[i];
                    if (action.Requirement is null || action.Requirement())
                        action.Method(currentMs);
                }
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        #endregion Private Methods
    }
}
