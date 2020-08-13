using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces.ModAPI.ColShape;
using TDS_Server.Data.Interfaces.ModAPI.Pool;

namespace TDS_Server.RAGEAPI.Pool
{
    internal class PoolColShapesAPI : IPoolColShapesAPI
    {
        #region Public Constructors

        public PoolColShapesAPI()
        {
            RAGE.Entities.Colshapes.CreateEntity = netHandle => new ColShape.ColShape(netHandle);
        }

        #endregion Public Constructors

        #region Public Properties

        public List<ITDSColShape> All => RAGE.Entities.Colshapes.All.OfType<ITDSColShape>().ToList();
        public IEnumerable<ITDSColShape> AsEnumerable => RAGE.Entities.Colshapes.AsEnumerable.OfType<ITDSColShape>();
        public int Count => RAGE.Entities.Colshapes.Count;

        #endregion Public Properties

        #region Public Methods

        public ITDSColShape? GetAt(ushort id)
            => RAGE.Entities.Colshapes.GetAt(id) as ITDSColShape;

        #endregion Public Methods
    }
}
