using TDS.Client.Handler.Entities.GTA;

namespace TDS.Client.Handler.Factories
{
    public class PickupFactory
    {
        public PickupFactory() => RAGE.Elements.Entities.Pickups.CreateEntity =
                (ushort id, ushort remoteId) => new TDSPickup(id, remoteId);
    }
}
