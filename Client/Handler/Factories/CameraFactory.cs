using TDS_Client.Handler.Entities.GTA;

namespace TDS_Client.Handler.Factories
{
    public class CameraFactory
    {
        public CameraFactory()
        {
            RAGE.Elements.Entities.Cameras.CreateEntity =
                (ushort id, ushort remoteId) => new TDSCamera(id, remoteId);
        }
    }
}
