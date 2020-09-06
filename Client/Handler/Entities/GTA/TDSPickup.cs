using TDS_Client.Data.Abstracts.Entities.GTA;

namespace TDS_Client.Handler.Entities.GTA
{
    public class TDSPickup : ITDSPickup
    {
        public TDSPickup(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
