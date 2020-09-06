namespace TDS_Client.Data.Abstracts.Entities.GTA
{
    public abstract class ITDSTextLabel : RAGE.Elements.TextLabel
    {
        public ITDSTextLabel(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
