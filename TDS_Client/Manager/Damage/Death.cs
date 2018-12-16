using RAGE.Elements;
using RAGE.Game;
using TDS_Client.Default;
using TDS_Client.Manager.Draw.Scaleform;
using TDS_Client.Manager.Utility;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Damage
{
    static class Death
    {
        public static void PlayerSpawn()
        {
            Cam.DoScreenFadeIn(Settings.ScreenFadeInTimeAfterSpawn);
        }

        public static void PlayerDeath(Player player)
        {
            if (player != Player.LocalPlayer)
                return;
            Cam.DoScreenFadeOut(Settings.ScreenFadeOutTimeAfterSpawn);
            Misc.IgnoreNextRestart(true);
            Misc.SetFadeOutAfterDeath(false);
            Audio.RequestScriptAudioBank(DAudioRef.HUD_MINI_GAME_SOUNDSET, true, 0);
            Audio.PlaySoundFrontend(-1, DAudioName.CHECKPOINT_NORMAL, DAudioRef.HUD_MINI_GAME_SOUNDSET, true);

            ScaleformMessage.ShowWastedMessage();
        }
    }
}
