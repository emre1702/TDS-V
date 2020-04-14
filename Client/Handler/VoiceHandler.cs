using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;

namespace TDS_Client.Handler
{
    public class VoiceHandler : ServiceBase
    {
        private readonly SettingsHandler _settingsHandler;
        private readonly BrowserHandler _browserHandler;
        private readonly UtilsHandler _utilsHandler;
        private readonly BindsHandler _bindsHandler;

        public VoiceHandler(IModAPI modAPI, LoggingHandler loggingHandler, BindsHandler bindsHandler, SettingsHandler settingsHandler, BrowserHandler browserHandler, 
            UtilsHandler utilsHandler, EventsHandler eventsHandler)
            : base(modAPI, loggingHandler)
        {
            if (!modAPI.Voice.Allowed)
                return;

            _settingsHandler = settingsHandler;
            _browserHandler = browserHandler;
            _utilsHandler = utilsHandler;
            _bindsHandler = bindsHandler;

            eventsHandler.LoggedIn += EventsHandler_LoggedIn;
            eventsHandler.SettingsLoaded += EventsHandler_SettingsLoaded;
            eventsHandler.PlayerJoinedSameLobby += SetForPlayer;
        }

        private void SetForPlayer(IPlayer player)
        {
            if (!ModAPI.Voice.Allowed)
                return;

            player.AutoVolume = _settingsHandler.PlayerSettings.VoiceAutoVolume;
            if (!_settingsHandler.PlayerSettings.VoiceAutoVolume)
                player.VoiceVolume = _settingsHandler.PlayerSettings.VoiceVolume;
            player.Voice3d = _settingsHandler.PlayerSettings.Voice3D;
        }

        private void Start(Control _)
        {
            if (!ModAPI.Voice.Allowed)
                return;

            if (_browserHandler.InInput)
                return;

            ModAPI.Voice.Muted = false;
            _browserHandler.PlainMain.StartPlayerTalking(_utilsHandler.GetDisplayName(ModAPI.LocalPlayer));
        }

        private void Stop(Control _)
        {
            ModAPI.Voice.Muted = true;
            _browserHandler.PlainMain.StopPlayerTalking(_utilsHandler.GetDisplayName(ModAPI.LocalPlayer));
        }

        private void EventsHandler_LoggedIn()
        {
            _bindsHandler.Add(Control.PushToTalk, Start, KeyPressState.Down);
            _bindsHandler.Add(Control.PushToTalk, Stop, KeyPressState.Up);
        }

        private void EventsHandler_SettingsLoaded()
        {
            foreach (var player in ModAPI.Pool.Players.All)
            {
                SetForPlayer(player);
            }
        }
    }
}
