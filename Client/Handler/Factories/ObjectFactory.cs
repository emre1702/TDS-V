using TDS.Client.Handler.Entities.GTA;

namespace TDS.Client.Handler.Factories
{
    public class ObjectFactory
    {
        public ObjectFactory() =>
            RAGE.Elements.Entities.Objects.CreateEntity =
                (ushort id, ushort remoteId) => new TDSObject(id, remoteId);
    }
}
