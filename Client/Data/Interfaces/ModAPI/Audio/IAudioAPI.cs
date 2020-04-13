namespace TDS_Client.Data.Interfaces.ModAPI.Audio
{
    public interface IAudioAPI
    {
        void SetAudioFlag(string flagName, bool toggle);
        void PlaySoundFrontend(int soundId, string audioName, string audioRef);
    }
}
