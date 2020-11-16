using TDS.Client.Handler.Entities.GTA;

namespace TDS.Client.Handler.Factories
{
    public class ColshapeFactory
    {
        public ColshapeFactory()
            => RAGE.Elements.Entities.Colshapes.CreateEntity =
                (ushort id, ushort remoteId) => new TDSColshape(id, remoteId);
    }
}
