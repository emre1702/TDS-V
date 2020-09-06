using RAGE;

namespace TDS_Client.Data.Abstracts.Entities.GTA
{
    public abstract class ITDSPed : RAGE.Elements.Ped
    {
        public Vector3 Rotation
        {
            get => GetRotation(2);
            set => SetRotation(value.X, value.Y, value.Z, 2, true);
        }

        public ITDSPed(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }

        public ITDSPed(uint model, Vector3 position, float heading = 0, uint dimension = 0) : base(model, position, heading, dimension)
        {
        }
    }
}
