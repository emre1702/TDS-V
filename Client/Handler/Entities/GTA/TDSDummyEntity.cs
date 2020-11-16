using TDS.Client.Data.Abstracts.Entities.GTA;

namespace TDS.Client.Handler.Entities.GTA
{
    public class TDSDummyEntity : ITDSDummyEntity
    {
        public TDSDummyEntity(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
