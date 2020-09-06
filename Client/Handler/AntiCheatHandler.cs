using System.Collections.Generic;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Deathmatch;
using static RAGE.Events;

namespace TDS_Client.Handler
{
    public class AntiCheatHandler : ServiceBase
    {
        private readonly PlayerFightHandler _playerFightHandler;

        public AntiCheatHandler(LoggingHandler loggingHandler, PlayerFightHandler playerFightHandler)
            : base(loggingHandler)
        {
            _playerFightHandler = playerFightHandler;
            Tick += OnTick;
        }

        public void OnTick(List<TickNametagData> _)
        {
            RAGE.Game.Player.SetPlayerTargetingMode(0);
            RAGE.Game.Player.SetPlayerLockon(false);

            if (_playerFightHandler.InFight)
            {
                if (RAGE.Game.Player.GetPlayerInvincible())
                {
                    Logging.LogToServer("Player is invincible, but shouldn't be.", "AntiCheatHandler");
                    RAGE.Game.Player.SetPlayerInvincible(false);
                }
            }
        }
    }
}
