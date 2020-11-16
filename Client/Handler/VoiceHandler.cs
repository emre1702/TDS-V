using RAGE.Game;
using System.Linq;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Data.Enums;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Events;
using TDS.Shared.Data.Models;

namespace TDS.Client.Handler
{
    public class VoiceHandler : ServiceBase
    {
        private readonly BindsHandler _bindsHandler;
        private readonly BrowserHandler _browserHandler;
        private readonly UtilsHandler _utilsHandler;
        private SyncedClientPlayerSettings _syncedClientPlayerSettings;

        public VoiceHandler(LoggingHandler loggingHandler, BindsHandler bindsHandler, BrowserHandler browserHandler,
            UtilsHandler utilsHandler, EventsHandler eventsHandler)
            : base(loggingHandler)
        {
            if (!RAGE.Voice.Allowed)
                return;

            _browserHandler = browserHandler;
            _utilsHandler = utilsHandler;
            _bindsHandler = bindsHandler;

            eventsHandler.LoggedIn += EventsHandler_LoggedIn;
            eventsHandler.SettingsLoaded += EventsHandler_SettingsLoaded;
            eventsHandler.PlayerJoinedSameLobby += SetForPlayer;
        }

        private void EventsHandler_LoggedIn()
        {
            _bindsHandler.Add(Control.PushToTalk, Start, KeyPressState.Down);
            _bindsHandler.Add(Control.PushToTalk, Stop, KeyPressState.Up);
        }

        private void EventsHandler_SettingsLoaded(SyncedClientPlayerSettings settings)
        {
            _syncedClientPlayerSettings = settings;

            foreach (var player in RAGE.Elements.Entities.Players.All.OfType<ITDSPlayer>())
            {
                SetForPlayer(player);
            }
        }

        private void SetForPlayer(ITDSPlayer player)
        {
            if (!RAGE.Voice.Allowed)
                return;
            if (_syncedClientPlayerSettings is null)
                return;

            player.AutoVolume = _syncedClientPlayerSettings.VoiceAutoVolume;
            if (!_syncedClientPlayerSettings.VoiceAutoVolume)
                player.VoiceVolume = _syncedClientPlayerSettings.VoiceVolume;
            player.Voice3d = _syncedClientPlayerSettings.Voice3D;
        }

        private void Start(Control _)
        {
            if (!RAGE.Voice.Allowed)
                return;

            if (_browserHandler.InInput)
                return;

            RAGE.Voice.Muted = false;
            _browserHandler.PlainMain.StartPlayerTalking(_utilsHandler.GetDisplayName(RAGE.Elements.Player.LocalPlayer as ITDSPlayer));
        }

        private void Stop(Control _)
        {
            RAGE.Voice.Muted = true;
            _browserHandler.PlainMain.StopPlayerTalking(_utilsHandler.GetDisplayName(RAGE.Elements.Player.LocalPlayer as ITDSPlayer));
        }
    }
}
