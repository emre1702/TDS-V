using TDS_Client.Data.Interfaces.RAGE.Game.Audio;

namespace TDS_Client.RAGEAPI.Audio
{
    internal class AudioAPI : IAudioAPI
    {
        #region Public Methods

        public void PlaySoundFrontend(int soundId, string audioName, string audioRef)
        {
            RAGE.Game.Audio.PlaySoundFrontend(soundId, audioName, audioRef, true);
        }

        public void SetAudioFlag(string flagName, bool toggle)
        {
            RAGE.Game.Audio.SetAudioFlag(flagName, toggle);
        }

        #endregion Public Methods
    }
}