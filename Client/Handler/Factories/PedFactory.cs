using TDS.Client.Handler.Entities.GTA;

namespace TDS.Client.Handler.Factories
{
    public class PedFactory
    {
        public PedFactory()
            => RAGE.Elements.Entities.Peds.CreateEntity =
                (ushort id, ushort remoteId) => new TDSPed(id, remoteId);
    }
}
