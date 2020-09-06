using TDS_Client.Data.Abstracts.Entities.GTA;

namespace TDS_Client.Handler.Entities.GTA
{
    public class TDSDummyEntity : ITDSDummyEntity
    {
        public TDSDummyEntity(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}
