using System;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler;
using TDS_Client.RAGEAPI.Player;

namespace TDS_Client.RAGEAPI.Event
{
    class PlayerStopTalkingEventHandler : BaseEventHandler<PlayerDelegate>
    {
        private readonly LoggingHandler _loggingHandler;
        private readonly PlayerConvertingHandler _playerConvertingHandler;

        public PlayerStopTalkingEventHandler(LoggingHandler loggingHandler, PlayerConvertingHandler playerConvertingHandler)
            : base()
        {
            _loggingHandler = loggingHandler;
            _playerConvertingHandler = playerConvertingHandler;

            RAGE.Events.OnPlayerStopTalking += PlayerStopTalking;
        }

        private void PlayerStopTalking(RAGE.Elements.Player playerMod)
        {
            if (Actions.Count == 0)
                return;

            try
            {
                IPlayer player = _playerConvertingHandler.GetPlayer(playerMod);

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
    }
}
