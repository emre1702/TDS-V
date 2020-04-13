using TDS_Client.Data.Interfaces.ModAPI.Audio;

namespace TDS_Client.RAGEAPI.Audio
{
    class AudioAPI : IAudioAPI
    {
        public void PlaySoundFrontend(int soundId, string audioName, string audioRef)
        {
            RAGE.Game.Audio.PlaySoundFrontend(soundId, audioName, audioRef, true);
        }

        public void SetAudioFlag(string flagName, bool toggle)
        {
            RAGE.Game.Audio.SetAudioFlag(flagName, toggle);
        }
    }
}
