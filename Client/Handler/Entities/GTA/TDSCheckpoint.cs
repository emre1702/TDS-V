using TDS.Client.Data.Abstracts.Entities.GTA;

namespace TDS.Client.Handler.Entities.GTA
{
    public class TDSCheckpoint : ITDSCheckpoint
    {
        public TDSCheckpoint(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
