using System;
using TDS_Shared.Data.Models;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Draw;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Deathmatch
{
    public class DeathHandler
    {
        private readonly IModAPI _modAPI;
        private readonly SettingsHandler _settingsHandler;
        private readonly ScaleformMessageHandler _scaleformMessageHandler;
        private readonly UtilsHandler _utilsHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly BrowserHandler _browserHandler;

        public DeathHandler(IModAPI modAPI, SettingsHandler settingsHandler, ScaleformMessageHandler scaleformMessageHandler, EventsHandler eventsHandler,
            UtilsHandler utilsHandler, BrowserHandler browserHandler)
        {
            _modAPI = modAPI;
            _settingsHandler = settingsHandler;
            _scaleformMessageHandler = scaleformMessageHandler;
            _utilsHandler = utilsHandler;
            _eventsHandler = eventsHandler;
            _browserHandler = browserHandler;

            modAPI.Event.Spawn.Add(new EventMethodData<SpawnDelegate>(_ => PlayerSpawn()));
            modAPI.Event.Death.Add(new EventMethodData<DeathDelegate>(PlayerDeath));

            modAPI.Event.Add(ToClientEvent.Death, OnDeathMethod);
            modAPI.Event.Add(ToClientEvent.ExplodeHead, OnExplodeHeadMethod);

            eventsHandler.LobbyJoined += EventsHandler_LobbyJoined;
        }

        public void PlayerSpawn()
        {
            _modAPI.Cam.DoScreenFadeIn(_settingsHandler.ScreenFadeInTimeAfterSpawn);
            _modAPI.Graphics.StopScreenEffect(EffectName.DEATHFAILMPIN);
            _modAPI.Cam.SetCamEffect(0);
        }

        public void PlayerDeath(IPlayer player, uint reason, IPlayer killer, CancelEventArgs cancel)
        {
            if (player != _modAPI.LocalPlayer)
                return;
            _modAPI.Cam.DoScreenFadeOut(_settingsHandler.ScreenFadeOutTimeAfterSpawn);
            _modAPI.Misc.IgnoreNextRestart(true);
            _modAPI.Misc.SetFadeOutAfterDeath(false);
            _modAPI.Audio.PlaySoundFrontend(-1, AudioName.BED, AudioRef.WASTEDSOUNDS);
            _modAPI.Cam.SetCamEffect(CamEffect.ZoomIn_Tilt30Deg_WobbleSlowly);
            _modAPI.Graphics.StartScreenEffect(EffectName.DEATHFAILMPIN, 0, true);

            _scaleformMessageHandler.ShowWastedMessage();
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
            IPlayer player = _utilsHandler.GetPlayerByHandleValue(Convert.ToUInt16(args[0]));
            bool willRespawn = Convert.ToBoolean(args[3]);
            if (player == _modAPI.LocalPlayer)
            {
                _eventsHandler.OnLocalPlayerDied();
            }
            int teamIndex = (int)args[1];
            _eventsHandler.OnPlayerDied(player, teamIndex, willRespawn);

            string killinfoStr = (string)args[2];
            _browserHandler.PlainMain.AddKillMessage(killinfoStr);
        }

        private void OnExplodeHeadMethod(object[] args)
        {
            var weaponHash = (WeaponHash)Convert.ToUInt32(args[0]);
            _modAPI.LocalPlayer.ExplodeHead(weaponHash);
        }
    }
}
