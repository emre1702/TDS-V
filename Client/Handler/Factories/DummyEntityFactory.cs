using TDS_Client.Handler.Entities.GTA;

namespace TDS_Client.Handler.Factories
{
    public class DummyEntityFactory
    {
        public DummyEntityFactory()
            => RAGE.Elements.Entities.DummyEntities.CreateEntity =
                (ushort id, ushort remoteId) => new TDSDummyEntity(id, remoteId);
    }
}
