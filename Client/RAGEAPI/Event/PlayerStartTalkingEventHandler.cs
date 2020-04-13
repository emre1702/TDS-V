using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.RAGEAPI.Player;

namespace TDS_Client.RAGEAPI.Event
{
    class PlayerStartTalkingEventHandler : BaseEventHandler<PlayerDelegate>
    {
        private readonly PlayerConvertingHandler _playerConvertingHandler;

        public PlayerStartTalkingEventHandler(PlayerConvertingHandler playerConvertingHandler)
            : base()
        {
            _playerConvertingHandler = playerConvertingHandler;

            RAGE.Events.OnPlayerStartTalking += PlayerStartTalking;
        }

        private void PlayerStartTalking(RAGE.Elements.Player playerMod)
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
