using TDS_Client.Data.Interfaces.ModAPI.Vehicle;

namespace TDS_Client.RAGEAPI.Vehicle
{
    class Vehicle : Entity.EntityBase, IVehicle
    {
        private readonly RAGE.Elements.Vehicle _instance;

        public Vehicle(RAGE.Elements.Vehicle instance) : base(instance)
            => _instance = instance;
    }
}
