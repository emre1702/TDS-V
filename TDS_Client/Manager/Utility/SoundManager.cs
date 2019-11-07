using System;
using System.Collections.Generic;
using System.Text;
using TDS_Common.Enum;

namespace TDS_Client.Manager.Utility
{
    static class SoundManager
    {
        public static void PlaySound(ESound sound)
        {
            if (!ClientConstants.SoundPaths.TryGetValue(sound, out string path))
            {
                RAGE.Chat.Output(string.Format(Settings.Language.ERROR, "PlaySound: " + sound.ToString()));
                return;
            }
            
            //TODO: Play sound
        }
    }
}
