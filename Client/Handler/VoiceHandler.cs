using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;

namespace TDS_Client.Handler
{
    public class VoiceHandler
    {
        private readonly SettingsHandler _settingsHandler;
        private readonly BrowserHandler _browserHandler;
        private readonly IModAPI _modAPI;
        private readonly UtilsHandler _utilsHandler;

        public VoiceHandler(BindsHandler bindsHandler, SettingsHandler settingsHandler, BrowserHandler browserHandler, IModAPI modAPI, UtilsHandler utilsHandler, EventsHandler eventsHandler)
        {
            _settingsHandler = settingsHandler;
            _browserHandler = browserHandler;
            _modAPI = modAPI;
            _utilsHandler = utilsHandler;

            //if (!Voice.Allowed)
            //    return;
            bindsHandler.Add(Control.PushToTalk, Start, KeyPressState.Down);
            bindsHandler.Add(Control.PushToTalk, Stop, KeyPressState.Up);

            eventsHandler.SettingsLoaded += EventsHandler_SettingsLoaded;
        }

        public void SetForPlayer(IPlayer player)
        {
            player.AutoVolume = _settingsHandler.PlayerSettings.VoiceAutoVolume;
            if (!_settingsHandler.PlayerSettings.VoiceAutoVolume)
                player.VoiceVolume = _settingsHandler.PlayerSettings.VoiceVolume;
            player.Voice3d = _settingsHandler.PlayerSettings.Voice3D;
        }

        private void Start(Control _)
        {
            if (_browserHandler.InInput)
                return;

            _modAPI.Voice.Muted = false;
            _browserHandler.PlainMain.StartPlayerTalking(_utilsHandler.GetDisplayName(_modAPI.LocalPlayer));
        }

        private void Stop(Control _)
        {
            _modAPI.Voice.Muted = true;
            _browserHandler.PlainMain.StopPlayerTalking(_utilsHandler.GetDisplayName(_modAPI.LocalPlayer));
        }

        private void EventsHandler_SettingsLoaded()
        {
            foreach (var player in _modAPI.Pool.Players.All)
            {
                SetForPlayer(player);
            }
        }
    }
}
