using TDS_Server.Database.Entity.GangEntities;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.Entities.Gang
{
    public interface IGangHouse
    {
        #region Public Properties

        GangHouses Entity { get; }
        Position Position { get; }
        float SpawnRotation { get; }

        #endregion Public Properties
    }
}
