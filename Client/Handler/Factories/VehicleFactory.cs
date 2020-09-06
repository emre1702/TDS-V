using TDS_Client.Handler.Entities.GTA;

namespace TDS_Client.Handler.Factories
{
    public class VehicleFactory
    {
        public VehicleFactory() => RAGE.Elements.Entities.Vehicles.CreateEntity =
                (ushort id, ushort remoteId) => new TDSVehicle(id, remoteId);
    }
}
