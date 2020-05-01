using TDS_Server.Data.Interfaces.ModAPI.Ped;

namespace TDS_Server.RAGEAPI.Ped
{
    class Ped : PedBase, IPed
    {
        private readonly GTANetworkAPI.Ped _instance;

        public Ped(GTANetworkAPI.Ped instance) : base(instance)
            => _instance = instance;
    }
}
