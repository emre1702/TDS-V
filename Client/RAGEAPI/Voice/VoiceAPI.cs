using TDS_Client.Data.Interfaces.RAGE.Game.Voice;

namespace TDS_Client.RAGEAPI.Voice
{
    internal class VoiceAPI : IVoiceAPI
    {
        #region Public Properties

        public bool Allowed
        {
            get => RAGE.Voice.Allowed;
        }

        public bool Muted
        {
            get => RAGE.Voice.Muted;
            set => RAGE.Voice.Muted = value;
        }

        #endregion Public Properties
    }
}