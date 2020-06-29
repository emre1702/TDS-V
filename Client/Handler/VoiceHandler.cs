using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Models;

namespace TDS_Client.Handler
{
    public class VoiceHandler : ServiceBase
    {
        #region Private Fields

        private readonly BindsHandler _bindsHandler;
        private readonly BrowserHandler _browserHandler;
        private readonly UtilsHandler _utilsHandler;
        private SyncedPlayerSettingsDto _syncedPlayerSettingsDto;

        #endregion Private Fields

        #region Public Constructors

        public VoiceHandler(IModAPI modAPI, LoggingHandler loggingHandler, BindsHandler bindsHandler, BrowserHandler browserHandler,
            UtilsHandler utilsHandler, EventsHandler eventsHandler)
            : base(modAPI, loggingHandler)
        {
            if (!modAPI.Voice.Allowed)
                return;

            _browserHandler = browserHandler;
            _utilsHandler = utilsHandler;
            _bindsHandler = bindsHandler;

            eventsHandler.LoggedIn += EventsHandler_LoggedIn;
            eventsHandler.SettingsLoaded += EventsHandler_SettingsLoaded;
            eventsHandler.PlayerJoinedSameLobby += SetForPlayer;
        }

        #endregion Public Constructors

        #region Private Methods

        private void EventsHandler_LoggedIn()
        {
            _bindsHandler.Add(Control.PushToTalk, Start, KeyPressState.Down);
            _bindsHandler.Add(Control.PushToTalk, Stop, KeyPressState.Up);
        }

        private void EventsHandler_SettingsLoaded(SyncedPlayerSettingsDto settings)
        {
            _syncedPlayerSettingsDto = settings;

            foreach (var player in ModAPI.Pool.Players.All)
            {
                SetForPlayer(player);
            }
        }

        private void SetForPlayer(IPlayer player)
        {
            if (!ModAPI.Voice.Allowed)
                return;
            if (_syncedPlayerSettingsDto is null)
                return;

            player.AutoVolume = _syncedPlayerSettingsDto.VoiceAutoVolume;
            if (!_syncedPlayerSettingsDto.VoiceAutoVolume)
                player.VoiceVolume = _syncedPlayerSettingsDto.VoiceVolume;
            player.Voice3d = _syncedPlayerSettingsDto.Voice3D;
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

        #endregion Private Methods
    }
}
