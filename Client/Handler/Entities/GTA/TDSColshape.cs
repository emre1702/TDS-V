using TDS.Client.Data.Abstracts.Entities.GTA;

namespace TDS.Client.Handler.Entities.GTA
{
    public class TDSColshape : ITDSColshape
    {
        public TDSColshape(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
