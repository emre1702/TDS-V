namespace TDS_Client.Data.Abstracts.Entities.GTA
{
    public abstract class ITDSCamera : RAGE.Elements.Camera
    {
        public ITDSCamera(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
