using TDS.Client.Data.Abstracts.Entities.GTA;

namespace TDS.Client.Handler.Entities.GTA
{
    public class TDSTextLabel : ITDSTextLabel
    {
        public TDSTextLabel(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
