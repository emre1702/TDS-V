using TDS_Client.Handler.Entities.GTA;

namespace TDS_Client.Handler.Factories
{
    public class PedFactory
    {
        public PedFactory()
        {
            RAGE.Elements.Entities.Peds.CreateEntity =
                (ushort id, ushort remoteId) => new TDSPed(id, remoteId);
        }
    }
}
