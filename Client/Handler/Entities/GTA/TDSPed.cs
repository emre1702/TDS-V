using RAGE;
using TDS.Client.Data.Abstracts.Entities.GTA;

namespace TDS.Client.Handler.Entities.GTA
{
    public class TDSPed : ITDSPed
    {
        public TDSPed(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }

        public TDSPed(uint model, Vector3 position, float heading = 0, uint dimension = 0) : base(model, position, heading, dimension)
        {
        }
    }
}
