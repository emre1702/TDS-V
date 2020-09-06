using TDS_Client.Handler.Entities.GTA;

namespace TDS_Client.Handler.Factories
{
    public class ColshapeFactory
    {
        public ColshapeFactory()
            => RAGE.Elements.Entities.Colshapes.CreateEntity =
                (ushort id, ushort remoteId) => new TDSColshape(id, remoteId);
    }
}
