using TDS.Client.Data.Defaults;
using TDS.Client.Handler;
using TDS.Shared.Data.Enums;

namespace TDS.Client.Manager.Utility
{
    public class SoundsHandler : ServiceBase
    {

        private readonly SettingsHandler _settingsHandler;

        public SoundsHandler(LoggingHandler loggingHandler, SettingsHandler settingsHandler)
            : base(loggingHandler) => _settingsHandler = settingsHandler;

        public void PlaySound(Sound sound)
        {
            if (!Constants.SoundPaths.TryGetValue(sound, out string path))
            {
                RAGE.Chat.Output(string.Format(_settingsHandler.Language.ERROR, "PlaySound: " + sound.ToString()));
                return;
            }

            //TODO: Play sound
        }

    }
}
