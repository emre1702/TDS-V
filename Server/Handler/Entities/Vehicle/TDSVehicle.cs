using System.Diagnostics.CodeAnalysis;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;

namespace TDS_Server.Handler.Entities.Vehicle
{
    public partial class TDSVehicle : ITDSVehicle
    {
        public IVehicle? Vehicle { get; }

        private readonly TDSVehicleHandler _tdsVehicleHandler;

        public TDSVehicle(IVehicle vehicle, TDSVehicleHandler tdsVehicleHandler)
        {
            _tdsVehicleHandler = tdsVehicleHandler;

            Vehicle = vehicle;
        }

        

        public void Delete()
        {
            if (Vehicle is null)
                return;

            _tdsVehicleHandler.Remove(Vehicle);
            Vehicle.Delete();
        }

        public bool Equals([AllowNull] ITDSVehicle other)
        {
            return Vehicle?.Id == other?.Vehicle?.Id;
        }
    }
}
