using TDS_Client.Data.Defaults;
using TDS_Client.Handler;
using TDS_Shared.Data.Enums;

namespace TDS_Client.Manager.Utility
{
    public class SoundsHandler : ServiceBase
    {
        #region Private Fields

        private readonly SettingsHandler _settingsHandler;

        #endregion Private Fields

        #region Public Constructors

        public SoundsHandler(LoggingHandler loggingHandler, SettingsHandler settingsHandler)
            : base(loggingHandler)
        {
            _settingsHandler = settingsHandler;
        }

        #endregion Public Constructors

        #region Public Methods

        public void PlaySound(Sound sound)
        {
            if (!Constants.SoundPaths.TryGetValue(sound, out string path))
            {
                RAGE.Chat.Output(string.Format(_settingsHandler.Language.ERROR, "PlaySound: " + sound.ToString()));
                return;
            }

            //TODO: Play sound
        }

        #endregion Public Methods
    }
}
