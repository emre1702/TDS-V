using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Deathmatch;

namespace TDS_Client.Handler
{
    public class AntiCheatHandler : ServiceBase
    {
        #region Private Fields

        private readonly PlayerFightHandler _playerFightHandler;

        #endregion Private Fields

        #region Public Constructors

        public AntiCheatHandler(IModAPI modAPI, LoggingHandler loggingHandler, PlayerFightHandler playerFightHandler)
            : base(modAPI, loggingHandler)
        {
            _playerFightHandler = playerFightHandler;

            modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(OnTick));
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnTick(int currentMs)
        {
            ModAPI.Player.SetPlayerTargetingMode(0);
            ModAPI.Player.SetPlayerLockon(false);

            if (_playerFightHandler.InFight)
            {
                if (ModAPI.Player.GetPlayerInvincible())
                {
                    Logging.LogToServer("Player is invincible, but shouldn't be.", "AntiCheatHandler");
                    ModAPI.Player.SetPlayerInvincible(false);
                }
            }
        }

        #endregion Public Methods
    }
}
