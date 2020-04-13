using TDS_Shared.Data.Models;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.RAGEAPI.Player;

namespace TDS_Client.RAGEAPI.Event
{
    public class DeathEventHandler : BaseEventHandler<DeathDelegate>
    {
        private readonly PlayerConvertingHandler _playerConvertingHandler;

        public DeathEventHandler(PlayerConvertingHandler playerConvertingHandler) : base()
        {
            _playerConvertingHandler = playerConvertingHandler;

            RAGE.Events.OnPlayerDeath += PlayerDeath;
        }

        private void PlayerDeath(RAGE.Elements.Player modPlayer, uint reason, RAGE.Elements.Player modKiller, RAGE.Events.CancelEventArgs modCancel)
        {
            if (Actions.Count == 0)
                return;

            var player = _playerConvertingHandler.GetPlayer(modPlayer);
            var killer = _playerConvertingHandler.GetPlayer(modKiller);
            var cancel = new CancelEventArgs();

            foreach (var action in Actions)
                if (action.Requirement is null || action.Requirement())
                    action.Method(player, reason, killer, cancel);

            modCancel.Cancel = cancel.Cancel;
        }
    }
}
