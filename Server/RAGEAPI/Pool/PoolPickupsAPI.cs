using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces.ModAPI.Pickup;
using TDS_Server.Data.Interfaces.ModAPI.Pool;

namespace TDS_Server.RAGEAPI.Pool
{
    internal class PoolPickupsAPI : IPoolPickupsAPI
    {
        #region Public Constructors

        public PoolPickupsAPI()
        {
            RAGE.Entities.Pickups.CreateEntity = netHandle => new Pickup.Pickup(netHandle);
        }

        #endregion Public Constructors

        #region Public Properties

        public List<IPickup> All => RAGE.Entities.Pickups.All.OfType<IPickup>().ToList();
        public IEnumerable<IPickup> AsEnumerable => RAGE.Entities.Pickups.AsEnumerable.OfType<IPickup>();
        public int Count => RAGE.Entities.Pickups.Count;

        #endregion Public Properties

        #region Public Methods

        public IPickup? GetAt(ushort id)
            => RAGE.Entities.Pickups.GetAt(id) as IPickup;

        #endregion Public Methods
    }
}
