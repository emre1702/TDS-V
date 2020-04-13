using TDS_Client.Data.Interfaces.ModAPI.Ped;

namespace TDS_Client.RAGEAPI.Ped
{
    class Ped : PedBase, IPed
    {
        private readonly RAGE.Elements.Ped _instance;

        public Ped(RAGE.Elements.Ped instance) : base(instance)
            => _instance = instance;

    }
}
