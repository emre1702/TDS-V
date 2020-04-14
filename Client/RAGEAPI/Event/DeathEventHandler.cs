using TDS_Shared.Data.Models;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.RAGEAPI.Player;
using TDS_Client.Handler;
using System;

namespace TDS_Client.RAGEAPI.Event
{
    public class DeathEventHandler : BaseEventHandler<DeathDelegate>
    {
        private readonly LoggingHandler _loggingHandler;
        private readonly PlayerConvertingHandler _playerConvertingHandler;

        public DeathEventHandler(LoggingHandler loggingHandler, PlayerConvertingHandler playerConvertingHandler) : base()
        {
            _loggingHandler = loggingHandler;
            _playerConvertingHandler = playerConvertingHandler;

            RAGE.Events.OnPlayerDeath += PlayerDeath;
        }

        private void PlayerDeath(RAGE.Elements.Player modPlayer, uint reason, RAGE.Elements.Player modKiller, RAGE.Events.CancelEventArgs modCancel)
        {
            if (Actions.Count == 0)
                return;

            try
            {
                var player = _playerConvertingHandler.GetPlayer(modPlayer);
                var killer = _playerConvertingHandler.GetPlayer(modKiller);
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
    }
}
