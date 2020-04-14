using System;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Deathmatch;

namespace TDS_Client.Handler
{
    public class AntiCheatHandler : ServiceBase
    {
        private readonly PlayerFightHandler _playerFightHandler;

        public AntiCheatHandler(IModAPI modAPI, LoggingHandler loggingHandler, PlayerFightHandler playerFightHandler)
            : base(modAPI, loggingHandler)
        {
            _playerFightHandler = playerFightHandler;

            modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(OnTick));
        }

        public void OnTick(int currentMs)
        {
            ModAPI.Player.SetPlayerTargetingMode(0);
            ModAPI.Player.SetPlayerLockon(false);

            if (_playerFightHandler.InFight)
            {
                if (ModAPI.Player.GetPlayerInvincible())
                {
                    //Todo: Log to server
                    ModAPI.Player.SetPlayerInvincible(false);
                }
            }
        }
    }
}
