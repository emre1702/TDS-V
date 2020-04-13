using System;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Deathmatch;

namespace TDS_Client.Handler
{
    public class AntiCheatHandler
    {
        private readonly IModAPI _modAPI;
        private readonly PlayerFightHandler _playerFightHandler;

        public AntiCheatHandler(IModAPI modAPI, PlayerFightHandler playerFightHandler)
        {
            _modAPI = modAPI;
            _playerFightHandler = playerFightHandler;

            modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(OnTick));
        }

        public void OnTick(int currentMs)
        {
            _modAPI.Player.SetPlayerTargetingMode(0);
            _modAPI.Player.SetPlayerLockon(false);

            if (_playerFightHandler.InFight)
            {
                if (_modAPI.Player.GetPlayerInvincible())
                {
                    //Todo: Log to server
                    _modAPI.Player.SetPlayerInvincible(false);
                }
            }
        }
    }
}
