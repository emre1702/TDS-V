using TDS_Client.Handler.Entities.GTA;

namespace TDS_Client.Handler.Factories
{
    public class BlipFactory
    {
        public BlipFactory() => RAGE.Elements.Entities.Blips.CreateEntity =
                (ushort id, ushort remoteId) => new TDSBlip(id, remoteId);
    }
}
