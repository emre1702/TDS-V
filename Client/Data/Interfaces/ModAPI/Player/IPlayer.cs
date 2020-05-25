using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Client.Data.Interfaces.ModAPI.Vehicle;

namespace TDS_Client.Data.Interfaces.ModAPI.Player
{
    public interface IPlayer : IPedBase
    {
        #region Public Properties

        bool AutoVolume { get; set; }
        bool IsTalking { get; }
        bool IsTypingInTextChat { get; }
        string Name { get; }
        IVehicle Vehicle { get; }
        bool Voice3d { get; set; }
        float VoiceVolume { get; set; }

        #endregion Public Properties

        #region Public Methods

        float GetVoiceAttribute(int attribute);

        void SetVoiceAttribute(int attribute, float value);

        #endregion Public Methods
    }
}
