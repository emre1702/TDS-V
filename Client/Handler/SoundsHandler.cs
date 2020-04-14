using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler;
using TDS_Shared.Data.Enums;

namespace TDS_Client.Manager.Utility
{
    public class SoundsHandler : ServiceBase
    {
        private readonly SettingsHandler _settingsHandler;

        public SoundsHandler(IModAPI modAPI, LoggingHandler loggingHandler, SettingsHandler settingsHandler)
            : base(modAPI, loggingHandler)
        {
            _settingsHandler = settingsHandler;
        }

        public void PlaySound(Sound sound)
        {
            if (!Constants.SoundPaths.TryGetValue(sound, out string path))
            {
                ModAPI.Chat.Output(string.Format(_settingsHandler.Language.ERROR, "PlaySound: " + sound.ToString()));
                return;
            }

            //TODO: Play sound
        }
    }
}
