using GTANetworkAPI;
using TDS.Server.Database.Entity.GangEntities;

namespace TDS.Server.Data.Interfaces
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
