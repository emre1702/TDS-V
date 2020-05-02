using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Interfaces.ModAPI.Vehicle;
using TDS_Client.RAGEAPI.Entity;
using TDS_Client.RAGEAPI.Ped;

namespace TDS_Client.RAGEAPI.Player
{
    class Player : PedBase, IPlayer
    {
        private readonly RAGE.Elements.Player _instance;

        private readonly EntityConvertingHandler _entityConvertingHandler;

        public Player(RAGE.Elements.Player instance, EntityConvertingHandler entityConvertingHandler) : base(instance)
            => (_instance, _entityConvertingHandler) = (instance, entityConvertingHandler);

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
        public bool IsTypingInTextChat => _instance.IsTypingInTextChat;


        public IVehicle Vehicle => _instance.Vehicle is null ? null : _entityConvertingHandler.GetEntity(_instance.Vehicle);

        public bool IsTalking => _instance.IsTalking;

        public float GetVoiceAttribute(int attribute)
            => _instance.GetVoiceAttribute(attribute);

        public void SetVoiceAttribute(int attribute, float value)
            => _instance.SetVoiceAttribute(attribute, value);
    }
}
