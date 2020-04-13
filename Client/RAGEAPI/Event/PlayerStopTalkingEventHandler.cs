using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.RAGEAPI.Player;

namespace TDS_Client.RAGEAPI.Event
{
    class PlayerStopTalkingEventHandler : BaseEventHandler<PlayerDelegate>
    {
        private readonly PlayerConvertingHandler _playerConvertingHandler;

        public PlayerStopTalkingEventHandler(PlayerConvertingHandler playerConvertingHandler)
            : base()
        {
            _playerConvertingHandler = playerConvertingHandler;

            RAGE.Events.OnPlayerStopTalking += PlayerStopTalking;
        }

        private void PlayerStopTalking(RAGE.Elements.Player playerMod)
        {
            if (Actions.Count == 0)
                return;

            IPlayer player = _playerConvertingHandler.GetPlayer(playerMod);

            foreach (var action in Actions)
                if (action.Requirement is null || action.Requirement())
                    action.Method(player);
        }
    }
}
