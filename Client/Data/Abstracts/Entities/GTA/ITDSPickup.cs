namespace TDS_Client.Data.Abstracts.Entities.GTA
{
    public abstract class ITDSPickup : RAGE.Elements.Pickup
    {
        public ITDSPickup(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
