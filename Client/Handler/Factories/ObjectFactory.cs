using TDS_Client.Handler.Entities.GTA;

namespace TDS_Client.Handler.Factories
{
    public class ObjectFactory
    {
        public ObjectFactory() =>
            RAGE.Elements.Entities.Objects.CreateEntity =
                (ushort id, ushort remoteId) => new TDSObject(id, remoteId);
    }
}
