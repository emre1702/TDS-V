using TDS_Client.Data.Abstracts.Entities.GTA;

namespace TDS_Client.Handler.Entities.GTA
{
    public class TDSPlayer : ITDSPlayer
    {
        public TDSPlayer(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
