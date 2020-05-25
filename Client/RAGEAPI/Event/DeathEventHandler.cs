using System;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler;
using TDS_Client.RAGEAPI.Player;
using TDS_Shared.Data.Models;

namespace TDS_Client.RAGEAPI.Event
{
    public class DeathEventHandler : BaseEventHandler<DeathDelegate>
    {
        #region Private Fields

        private readonly LoggingHandler _loggingHandler;

        #endregion Private Fields

        #region Public Constructors

        public DeathEventHandler(LoggingHandler loggingHandler) : base()
        {
            _loggingHandler = loggingHandler;

            RAGE.Events.OnPlayerDeath += PlayerDeath;
        }

        #endregion Public Constructors

        #region Private Methods

        private void PlayerDeath(RAGE.Elements.Player modPlayer, uint reason, RAGE.Elements.Player modKiller, RAGE.Events.CancelEventArgs modCancel)
        {
            if (Actions.Count == 0)
                return;

            try
            {
                var player = modPlayer as IPlayer;
                var killer = modKiller as IPlayer;
                var cancel = new CancelEventArgs();

                for (int i = Actions.Count - 1; i >= 0; --i)
                {
                    var action = Actions[i];
                    if (action.Requirement is null || action.Requirement())
                        action.Method(player, reason, killer, cancel);
                }

                modCancel.Cancel = cancel.Cancel;
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        #endregion Private Methods
    }
}
