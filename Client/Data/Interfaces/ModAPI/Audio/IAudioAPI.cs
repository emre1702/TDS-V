namespace TDS_Client.Data.Interfaces.ModAPI.Audio
{
    public interface IAudioAPI
    {
        #region Public Methods

        void PlaySoundFrontend(int soundId, string audioName, string audioRef);

        void SetAudioFlag(string flagName, bool toggle);

        #endregion Public Methods
    }
}
