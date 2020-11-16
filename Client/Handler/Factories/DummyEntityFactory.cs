using TDS.Client.Handler.Entities.GTA;

namespace TDS.Client.Handler.Factories
{
    public class DummyEntityFactory
    {
        public DummyEntityFactory()
            => RAGE.Elements.Entities.DummyEntities.CreateEntity =
                (ushort id, ushort remoteId) => new TDSDummyEntity(id, remoteId);
    }
}
