using RAGE;
using RAGE.Game;
using System.Collections.Generic;
using TDS_Client.Enum;
using TDS_Common.Default;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Utility
{
    internal class VoiceManager
    {
        public static void Init()
        {
            //if (!Voice.Allowed)
            //    return;
            BindManager.Add(Control.PushToTalk, Start, EKeyPressState.Down);
            BindManager.Add(Control.PushToTalk, Stop, EKeyPressState.Up);
        }

        public static void SetForPlayer(Player player)
        {
            player.AutoVolume = Settings.PlayerSettings.VoiceAutoVolume;
            if (!Settings.PlayerSettings.VoiceAutoVolume)
                player.VoiceVolume = Settings.PlayerSettings.VoiceVolume;
            player.Voice3d = Settings.PlayerSettings.Voice3D;
        }

        private static void Start(Control _)
        {
            Voice.Muted = false;
        }

        private static void Stop(Control _)
        {
            Voice.Muted = true;
        }
    }
}