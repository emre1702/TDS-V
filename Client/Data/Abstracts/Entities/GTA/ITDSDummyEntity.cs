namespace TDS_Client.Data.Abstracts.Entities.GTA
{
    public abstract class ITDSDummyEntity : RAGE.Elements.DummyEntity
    {
        public ITDSDummyEntity(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
