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

        public List<IColShape> All => RAGE.Entities.Colshapes.All.OfType<IColShape>().ToList();
        public IEnumerable<IColShape> AsEnumerable => RAGE.Entities.Colshapes.AsEnumerable.OfType<IColShape>();
        public int Count => RAGE.Entities.Colshapes.Count;

        #endregion Public Properties

        #region Public Methods

        public IColShape? GetAt(ushort id)
            => RAGE.Entities.Colshapes.GetAt(id) as IColShape;

        #endregion Public Methods
    }
}
