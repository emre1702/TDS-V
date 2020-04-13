using TDS_Client.Data.Interfaces.ModAPI.Voice;

namespace TDS_Client.RAGEAPI.Voice
{
    class VoiceAPI : IVoiceAPI
    {
        public bool Muted 
        { 
            get => RAGE.Voice.Muted; 
            set => RAGE.Voice.Muted = value; 
        }

        public bool Allowed
        {
            get => RAGE.Voice.Allowed;
        }
    }
}
