namespace TDS_Client.Data.Abstracts.Entities.GTA
{
    public abstract class ITDSColshape : RAGE.Elements.Colshape
    {
        public ITDSColshape(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
