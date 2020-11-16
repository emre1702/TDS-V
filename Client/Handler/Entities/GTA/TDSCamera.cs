using TDS.Client.Data.Abstracts.Entities.GTA;

namespace TDS.Client.Handler.Entities.GTA
{
    public class TDSCamera : ITDSCamera
    {
        public TDSCamera(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
