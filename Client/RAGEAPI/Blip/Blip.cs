using TDS_Client.Data.Interfaces.ModAPI.Blip;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Blip
{
    class Blip : Entity.Entity, IBlip
    {
        private readonly RAGE.Elements.Blip _instance;

        public Blip(RAGE.Elements.Blip instance) : base(instance)
            => _instance = instance;

        public Position3D Rotation
        {
            set => _instance.SetRotation((int)value.Z);
        }
    }
}
