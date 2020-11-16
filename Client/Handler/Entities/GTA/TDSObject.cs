using RAGE;
using TDS.Client.Data.Abstracts.Entities.GTA;

namespace TDS.Client.Handler.Entities.GTA
{
    public class TDSObject : ITDSObject
    {
        public TDSObject(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }

        public TDSObject(int handle) : base(handle)
        {
        }

        public TDSObject(uint hash, Vector3 position, Vector3 rotation, int alpha = 255, uint dimension = 0)
            : base(hash, position, rotation, alpha, dimension) { }
    }
}
