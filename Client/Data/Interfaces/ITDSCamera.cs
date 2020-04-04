using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces
{
    public interface ITDSCamera
    {
        Position3D Position { get; set; }
        Position3D Rotation { get; set; }
    }
}
