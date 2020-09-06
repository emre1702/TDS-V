using RAGE;
using TDS_Client.Data.Abstracts.Entities.GTA;

namespace TDS_Client.Handler.Entities.GTA
{
    public class TDSVehicle : ITDSVehicle
    {
        public TDSVehicle(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }

        public TDSVehicle(uint hash, Vector3 position, float heading = 0, string numberPlate = "", int alpha = 255, bool locked = false, int primColor = 0, int secColor = 0, uint dimension = 0)
            : base(hash, position, heading, numberPlate, alpha, locked, primColor, secColor, dimension)
        {
        }
    }
}
