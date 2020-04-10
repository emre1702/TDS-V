using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Client.Data.Interfaces.ModAPI.Audio
{
    public interface IAudioAPI
    {
        void SetAudioFlag(string v1, bool v2);
        void PlaySoundFrontend(int v1, string v2, string v3, bool v4);
    }
}
