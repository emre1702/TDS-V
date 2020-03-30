using RAGE.Game;
using TDS_Client.Default;
using TDS_Client.Manager.Draw.Scaleform;
using TDS_Client.Manager.Event;
using TDS_Client.Manager.Utility;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Damage
{
    internal static class Death
    {
        public static void PlayerSpawn()
        {
            Cam.DoScreenFadeIn(Settings.ScreenFadeInTimeAfterSpawn);
            Graphics.StopScreenEffect(DEffectName.DEATHFAILMPIN);
            Cam.SetCamEffect(0);
        }

        public static void PlayerDeath(Player player)
        {
            if (player != Player.LocalPlayer)
                return;
            Cam.DoScreenFadeOut(Settings.ScreenFadeOutTimeAfterSpawn);
            Misc.IgnoreNextRestart(true);
            Misc.SetFadeOutAfterDeath(false);
            Audio.PlaySoundFrontend(-1, DAudioName.BED, DAudioRef.WASTEDSOUNDS, true);
            Cam.SetCamEffect(1);
            Graphics.StartScreenEffect(DEffectName.DEATHFAILMPIN, 0, true);

            ScaleformMessage.ShowWastedMessage();

            CustomEventManager.SetDead();
        }
    }
}