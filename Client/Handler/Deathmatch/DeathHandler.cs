using RAGE.Elements;
using System;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Data.Defaults;
using TDS.Client.Data.Enums;
using TDS.Client.Data.Models;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Draw;
using TDS.Client.Handler.Events;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models;
using TDS.Shared.Default;
using static RAGE.Events;

namespace TDS.Client.Handler.Deathmatch
{
    public class DeathHandler : ServiceBase
    {
        private readonly BrowserHandler _browserHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly ScaleformMessageHandler _scaleformMessageHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly UtilsHandler _utilsHandler;

        public DeathHandler(LoggingHandler loggingHandler, SettingsHandler settingsHandler, ScaleformMessageHandler scaleformMessageHandler,
            EventsHandler eventsHandler, UtilsHandler utilsHandler, BrowserHandler browserHandler)
           : base(loggingHandler)
        {
            _settingsHandler = settingsHandler;
            _scaleformMessageHandler = scaleformMessageHandler;
            _utilsHandler = utilsHandler;
            _eventsHandler = eventsHandler;
            _browserHandler = browserHandler;

            OnPlayerSpawn += PlayerSpawn;
            OnPlayerDeath += PlayerDeath;

            Add(ToClientEvent.Death, OnDeathMethod);
            Add(ToClientEvent.ExplodeHead, OnExplodeHeadMethod);

            eventsHandler.LobbyJoined += EventsHandler_LobbyJoined;
        }

        public void PlayerDeath(Player player, uint reason, Player killer, CancelEventArgs cancel)
        {
            if (player != Player.LocalPlayer)
                return;
            Logging.LogWarning("", "DeathHandler.PlayerDeath");
            RAGE.Game.Cam.DoScreenFadeOut(_settingsHandler.ScreenFadeOutTimeAfterSpawn);
            RAGE.Game.Misc.IgnoreNextRestart(true);
            RAGE.Game.Misc.SetFadeOutAfterDeath(false);
            RAGE.Game.Audio.PlaySoundFrontend(-1, AudioName.BED, AudioRef.WASTEDSOUNDS, true);
            RAGE.Game.Cam.SetCamEffect((int)CamEffect.ZoomIn_Tilt30Deg_WobbleSlowly);
            RAGE.Game.Graphics.StartScreenEffect(EffectName.DEATHFAILMPIN, 0, true);

            _scaleformMessageHandler.ShowWastedMessage();
        }

        public void PlayerSpawn(CancelEventArgs _ = null)
        {
            Logging.LogWarning("", "DeathHandler.PlayerSpawn");
            RAGE.Game.Cam.DoScreenFadeIn(_settingsHandler.ScreenFadeInTimeAfterSpawn);
            RAGE.Game.Graphics.StopScreenEffect(EffectName.DEATHFAILMPIN);
            RAGE.Game.Cam.SetCamEffect(0);

            _eventsHandler.OnSpawn();
        }

        private void EventsHandler_LobbyJoined(SyncedLobbySettings settings)
        {
            if (settings.Type == LobbyType.MainMenu)
            {
                PlayerSpawn();
            }
        }

        private void OnDeathMethod(object[] args)
        {
            var player = _utilsHandler.GetPlayerByHandleValue(Convert.ToUInt16(args[0]));
            bool willRespawn = Convert.ToBoolean(args[3]);
            if (player == RAGE.Elements.Player.LocalPlayer)
            {
                _eventsHandler.OnLocalPlayerDied();
            }
            int teamIndex = (int)args[1];
            _eventsHandler.OnPlayerDied(player, teamIndex, willRespawn);

            string killInfoJson = (string)args[2];
            _browserHandler.Angular.AddKillMessage(killInfoJson);
        }

        private void OnExplodeHeadMethod(object[] args)
        {
            var weaponHash = uint.Parse(args[0].ToString());
            Player.LocalPlayer.ExplodeHead(weaponHash);
        }
    }
}
