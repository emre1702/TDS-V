using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces.ModAPI.Ped;
using TDS_Server.Data.Interfaces.ModAPI.Pool;

namespace TDS_Server.RAGEAPI.Pool
{
    internal class PoolPedsAPI : IPoolPedsAPI
    {
        #region Public Constructors

        public PoolPedsAPI()
        {
            RAGE.Entities.Peds.CreateEntity = netHandle => new Ped.Ped(netHandle);
        }

        #endregion Public Constructors

        #region Public Properties

        public List<IPed> All => RAGE.Entities.Peds.All.OfType<IPed>().ToList();
        public IEnumerable<IPed> AsEnumerable => RAGE.Entities.Peds.AsEnumerable.OfType<IPed>();
        public int Count => RAGE.Entities.Peds.Count;

        #endregion Public Properties

        #region Public Methods

        public IPed? GetAt(ushort id)
            => RAGE.Entities.Peds.GetAt(id) as IPed;

        #endregion Public Methods
    }
}
