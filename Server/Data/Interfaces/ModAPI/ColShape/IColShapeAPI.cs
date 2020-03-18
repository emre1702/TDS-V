using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI.ColShape
{
    public interface IColShapeAPI
    {
        IColShape CreateSphere(Position3D position, double range, ILobby lobby);
    }
}
