using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Ped
{
    class Ped : PedBase, IPed
    {
        private readonly RAGE.Elements.Ped _instance;

        public Ped(RAGE.Elements.Ped instance) : base(instance)
            => _instance = instance;

        public override Position3D Position
        {
            get => RAGE.Game.Entity.GetEntityCoords(_instance.Handle, true).ToPosition3D();
            set => RAGE.Game.Entity.SetEntityCoordsNoOffset(_instance.Handle, value.X, value.Y, value.Z, true, true, true);
        }
    }
}
