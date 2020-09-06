using RAGE;

namespace TDS_Client.Data.Abstracts.Entities.GTA
{
    public abstract class ITDSVehicle : RAGE.Elements.Vehicle
    {
        public Vector3 Rotation
        {
            get => GetRotation(2);
            set => SetRotation(value.X, value.Y, value.Z, 2, true);
        }

        public ITDSVehicle(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }

        public ITDSVehicle(uint hash, Vector3 position, float heading = 0, string numberPlate = "", int alpha = 255, bool locked = false, int primColor = 0, int secColor = 0, uint dimension = 0)
            : base(hash, position, heading, numberPlate, alpha, locked, primColor, secColor, dimension)
        {
        }
    }
}
