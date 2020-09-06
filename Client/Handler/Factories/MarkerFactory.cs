using TDS_Client.Handler.Entities.GTA;

namespace TDS_Client.Handler.Factories
{
    public class MarkerFactory
    {
        public MarkerFactory() => RAGE.Elements.Entities.Markers.CreateEntity =
                (ushort id, ushort remoteId) => new TDSMarker(id, remoteId);
    }
}
