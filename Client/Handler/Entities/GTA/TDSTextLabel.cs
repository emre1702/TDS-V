using TDS_Client.Data.Abstracts.Entities.GTA;

namespace TDS_Client.Handler.Entities.GTA
{
    public class TDSTextLabel : ITDSTextLabel
    {
        public TDSTextLabel(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
