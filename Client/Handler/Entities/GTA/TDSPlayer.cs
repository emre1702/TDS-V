using TDS.Client.Data.Abstracts.Entities.GTA;

namespace TDS.Client.Handler.Entities.GTA
{
    public class TDSPlayer : ITDSPlayer
    {
        public TDSPlayer(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
