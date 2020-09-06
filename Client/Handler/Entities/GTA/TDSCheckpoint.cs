using TDS_Client.Data.Abstracts.Entities.GTA;

namespace TDS_Client.Handler.Entities.GTA
{
    public class TDSCheckpoint : ITDSCheckpoint
    {
        public TDSCheckpoint(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
