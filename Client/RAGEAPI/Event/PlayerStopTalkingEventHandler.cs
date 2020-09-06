using System;
using TDS_Client.Data.Interfaces.RAGE.Game.Event;
using TDS_Client.Data.Interfaces.RAGE.Game.Player;
using TDS_Client.Handler;
using TDS_Client.RAGEAPI.Player;

namespace TDS_Client.RAGEAPI.Event
{
    internal class PlayerStopTalkingEventHandler : BaseEventHandler<PlayerDelegate>
    {
        #region Private Fields

        private readonly LoggingHandler _loggingHandler;

        #endregion Private Fields

        #region Public Constructors

        public PlayerStopTalkingEventHandler(LoggingHandler loggingHandler)
            : base()
        {
            _loggingHandler = loggingHandler;

            RAGE.Events.OnPlayerStopTalking += PlayerStopTalking;
        }

        #endregion Public Constructors

        #region Private Methods

        private void PlayerStopTalking(RAGE.Elements.Player playerMod)
        {
            if (Actions.Count == 0)
                return;

            try
            {
                ITDSPlayer player = playerMod as ITDSPlayer;

                for (int i = Actions.Count - 1; i >= 0; --i)
                {
                    var action = Actions[i];
                    if (action.Requirement is null || action.Requirement())
                        action.Method(player);
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