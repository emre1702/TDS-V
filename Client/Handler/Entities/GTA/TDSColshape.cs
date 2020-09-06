using TDS_Client.Data.Abstracts.Entities.GTA;

namespace TDS_Client.Handler.Entities.GTA
{
    public class TDSColshape : ITDSColshape
    {
        public TDSColshape(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
