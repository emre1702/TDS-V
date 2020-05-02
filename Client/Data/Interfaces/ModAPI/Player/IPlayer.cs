using TDS_Client.Data.Interfaces.ModAPI.Blip;
using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Client.Data.Interfaces.ModAPI.Vehicle;
using TDS_Shared.Data.Enums;

namespace TDS_Client.Data.Interfaces.ModAPI.Player
{
    public interface IPlayer : IPedBase
    {
        string Name { get; }
        bool AutoVolume { get; set; }
        float VoiceVolume { get; set; }
        bool Voice3d { get; set; }

        IVehicle Vehicle { get; }
        bool IsTypingInTextChat { get; }
        bool IsTalking { get; }

        float GetVoiceAttribute(int attribute);
        void SetVoiceAttribute(int attribute, float value);


    }   
}
