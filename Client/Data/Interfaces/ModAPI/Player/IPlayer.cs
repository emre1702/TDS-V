using TDS_Client.Data.Interfaces.ModAPI.Blip;
using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Shared.Data.Enums;

namespace TDS_Client.Data.Interfaces.ModAPI.Player
{
    public interface IPlayer : IPedBase
    {
        string Name { get; }
        bool AutoVolume { get; set; }
        float VoiceVolume { get; set; }
        bool Voice3d { get; set; }
        bool IsPlaying { get; }
        bool IsFreeAiming { get; }

        void SetMaxArmor(int maxArmor);

        void DisablePlayerFiring(bool toggle);
        // RAGE.Game.Player.SetPlayerMaxArmour(Constants.MaxPossibleArmor);
    }
}
