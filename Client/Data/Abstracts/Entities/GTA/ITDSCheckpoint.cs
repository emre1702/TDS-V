namespace TDS.Client.Data.Abstracts.Entities.GTA
{
    public abstract class ITDSCheckpoint : RAGE.Elements.Checkpoint
    {
        public ITDSCheckpoint(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
