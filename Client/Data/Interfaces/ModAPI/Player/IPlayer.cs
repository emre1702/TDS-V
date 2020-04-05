using TDS_Client.Data.Interfaces.ModAPI.Ped;

namespace TDS_Client.Data.Interfaces.ModAPI.Player
{
    public interface IPlayer : IPedBase
    {
        string Name { get; }
        ushort RemoteId { get; }
        object AutoVolume { get; set; }
        float VoiceVolume { get; set; }
        object Voice3d { get; set; }
        bool IsPlaying { get; }
        bool IsClimbing { get; }
        bool IsFreeAiming { get; }

        void SetMaxArmor(int maxArmor);
        // RAGE.Game.Player.SetPlayerMaxArmour(Constants.MaxPossibleArmor);
    }
}
