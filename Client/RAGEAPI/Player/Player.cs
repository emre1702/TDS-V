using TDS_Client.Data.Interfaces.ModAPI.Blip;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.RAGEAPI.Ped;
using TDS_Shared.Data.Enums;

namespace TDS_Client.RAGEAPI.Player
{
    class Player : PedBase, IPlayer
    {
        private readonly RAGE.Elements.Player _instance;

        public Player(RAGE.Elements.Player instance) : base(instance)
            => _instance = instance;

        public string Name => _instance.Name;

        public bool AutoVolume 
        { 
            get => _instance.AutoVolume; 
            set => _instance.AutoVolume = value; 
        }
        public float VoiceVolume 
        { 
            get => _instance.VoiceVolume; 
            set => _instance.VoiceVolume = value; 
        }
        public bool Voice3d 
        { 
            get => _instance.Voice3d; 
            set => _instance.Voice3d = value; 
        }

        /** <summary>Only works for localplayer</summary> */
        public bool IsPlaying => RAGE.Game.Player.IsPlayerPlaying();

        /** <summary>Only works for localplayer</summary> */
        public bool IsFreeAiming => RAGE.Game.Player.IsPlayerFreeAiming();


        public void DisablePlayerFiring(bool toggle)
        {
            RAGE.Game.Player.DisablePlayerFiring(toggle);
        }

        public void SetMaxArmor(int maxArmor)
        {
            RAGE.Game.Player.SetPlayerMaxArmour(maxArmor);
        }
    }
}
