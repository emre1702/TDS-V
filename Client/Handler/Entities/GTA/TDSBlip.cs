using RAGE;
using TDS.Client.Data.Abstracts.Entities.GTA;

namespace TDS.Client.Handler.Entities.GTA
{
    public class TDSBlip : ITDSBlip
    {
        public TDSBlip(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }

        public TDSBlip(uint sprite, Vector3 position, string name = "", float scale = 1, int color = 0, int alpha = 255, float drawDistance = 0, bool shortRange = false, int rotation = 0, float radius = 0, uint dimension = 0)
            : base(sprite, position, name, scale, color, alpha, drawDistance, shortRange, rotation, radius, dimension)
        {
        }
    }
}
