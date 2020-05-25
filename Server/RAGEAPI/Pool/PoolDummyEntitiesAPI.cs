using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces.ModAPI.DummyEntity;
using TDS_Server.Data.Interfaces.ModAPI.Pool;

namespace TDS_Server.RAGEAPI.Pool
{
    internal class PoolDummyEntitiesAPI : IPoolDummyEntitiesAPI
    {
        #region Public Constructors

        public PoolDummyEntitiesAPI()
        {
            RAGE.Entities.DummyEntities.CreateEntity = netHandle => new Entity.DummyEntity(netHandle);
        }

        #endregion Public Constructors

        #region Public Properties

        public List<IDummyEntity> All => RAGE.Entities.DummyEntities.All.OfType<IDummyEntity>().ToList();
        public IEnumerable<IDummyEntity> AsEnumerable => RAGE.Entities.DummyEntities.AsEnumerable.OfType<IDummyEntity>();
        public int Count => RAGE.Entities.DummyEntities.Count;

        #endregion Public Properties

        #region Public Methods

        public IDummyEntity? GetAt(ushort id)
            => RAGE.Entities.DummyEntities.GetAt(id) as IDummyEntity;

        #endregion Public Methods
    }
}
