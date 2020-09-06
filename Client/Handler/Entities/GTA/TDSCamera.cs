using TDS_Client.Data.Abstracts.Entities.GTA;

namespace TDS_Client.Handler.Entities.GTA
{
    public class TDSCamera : ITDSCamera
    {
        public TDSCamera(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
