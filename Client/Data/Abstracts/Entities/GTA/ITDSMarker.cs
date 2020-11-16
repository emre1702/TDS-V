namespace TDS.Client.Data.Abstracts.Entities.GTA
{
    public abstract class ITDSMarker : RAGE.Elements.Marker
    {
        public ITDSMarker(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
