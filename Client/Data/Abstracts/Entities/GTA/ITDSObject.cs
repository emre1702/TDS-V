using RAGE;

namespace TDS_Client.Data.Abstracts.Entities.GTA
{
    public abstract class ITDSObject : RAGE.Elements.MapObject
    {
        public Vector3 Rotation
        {
            get => GetRotation(2);
            set => SetRotation(value.X, value.Y, value.Z, 2, true);
        }

        public ITDSObject(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }

        public ITDSObject(int handle) : base(handle)
        {
        }

        public ITDSObject(uint hash, Vector3 position, Vector3 rotation, int alpha = 255, uint dimension = 0)
            : base(hash, position, rotation, alpha, dimension) { }
    }
}
