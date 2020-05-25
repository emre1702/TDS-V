using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces.ModAPI.MapObject;
using TDS_Server.Data.Interfaces.ModAPI.Pool;

namespace TDS_Server.RAGEAPI.Pool
{
    internal class PoolMapObjectsAPI : IPoolMapObjectsAPI
    {
        #region Public Constructors

        public PoolMapObjectsAPI()
        {
            RAGE.Entities.Objects.CreateEntity = netHandle => new MapObject.MapObject(netHandle);
        }

        #endregion Public Constructors

        #region Public Properties

        public List<IMapObject> All => RAGE.Entities.Objects.All.OfType<IMapObject>().ToList();
        public IEnumerable<IMapObject> AsEnumerable => RAGE.Entities.Objects.AsEnumerable.OfType<IMapObject>();
        public int Count => RAGE.Entities.Objects.Count;

        #endregion Public Properties

        #region Public Methods

        public IMapObject? GetAt(ushort id)
            => RAGE.Entities.Objects.GetAt(id) as IMapObject;

        #endregion Public Methods
    }
}
