using System.ComponentModel;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Draw;

namespace TDS_Client.Handler.Deathmatch
{
    public class DeathHandler
    {
        private readonly IModAPI _modAPI;
        private readonly SettingsHandler _settingsHandler;
        private readonly ScaleformMessageHandler _scaleformMessageHandler;

        public DeathHandler(IModAPI modAPI, SettingsHandler settingsHandler, ScaleformMessageHandler scaleformMessageHandler)
        {
            _modAPI = modAPI;
            _settingsHandler = settingsHandler;
            _scaleformMessageHandler = scaleformMessageHandler;

            modAPI.Event.Death.Add(new EventMethodData<DeathDelegate>(PlayerDeath));
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
            _modAPI.Audio.PlaySoundFrontend(-1, AudioName.BED, AudioRef.WASTEDSOUNDS, true);
            _modAPI.Cam.SetCamEffect(1);
            _modAPI.Graphics.StartScreenEffect(EffectName.DEATHFAILMPIN, 0, true);

            _scaleformMessageHandler.ShowWastedMessage();
        }
    }
}
