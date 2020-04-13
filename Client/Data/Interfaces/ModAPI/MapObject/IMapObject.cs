using TDS_Client.Data.Interfaces.ModAPI.Entity;

namespace TDS_Client.Data.Interfaces.ModAPI.MapObject
{
    public interface IMapObject : IEntityBase
    {
        uint Model { get; set; }
    }
}
