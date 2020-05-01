using TDS_Server.Data.Interfaces.ModAPI.Ped;

namespace TDS_Server.RAGEAPI.Ped
{
    class PedBase : Entity.Entity, IPedBase
    {
        private readonly GTANetworkAPI.Entity _instance;

        public PedBase(GTANetworkAPI.Entity instance) : base(instance) 
        { 
            _instance = instance;
        }

        public int VehicleSeat => _instance is GTANetworkAPI.Player player ? player.VehicleSeat : -1;
    }
}
