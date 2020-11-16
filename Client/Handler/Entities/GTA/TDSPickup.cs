using TDS.Client.Data.Abstracts.Entities.GTA;

namespace TDS.Client.Handler.Entities.GTA
{
    public class TDSPickup : ITDSPickup
    {
        public TDSPickup(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
