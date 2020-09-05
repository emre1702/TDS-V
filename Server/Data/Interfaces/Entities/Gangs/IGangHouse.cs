using GTANetworkAPI;
using TDS_Server.Database.Entity.GangEntities;

namespace TDS_Server.Data.Interfaces
{
    public interface IGangHouse
    {
        #region Public Properties

        GangHouses Entity { get; }
        Vector3 Position { get; }
        float SpawnRotation { get; }

        #endregion Public Properties
    }
}
