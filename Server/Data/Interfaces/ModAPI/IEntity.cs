using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI
{
    public interface IEntity
    {
        Position3D Position { get; set; }
        uint Dimension { get; set; }
    }
}
