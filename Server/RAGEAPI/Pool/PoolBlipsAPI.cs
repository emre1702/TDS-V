using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces.ModAPI.Blip;
using TDS_Server.Data.Interfaces.ModAPI.Pool;

namespace TDS_Server.RAGEAPI.Pool
{
    internal class PoolBlipsAPI : IPoolBlipsAPI
    {
        #region Public Constructors

        public PoolBlipsAPI()
        {
            RAGE.Entities.Blips.CreateEntity = netHandle => new Blip.Blip(netHandle);
        }

        #endregion Public Constructors

        #region Public Properties

        public List<IBlip> All => RAGE.Entities.Blips.All.OfType<IBlip>().ToList();
        public IEnumerable<IBlip> AsEnumerable => RAGE.Entities.Blips.AsEnumerable.OfType<IBlip>();
        public int Count => RAGE.Entities.Blips.Count;

        #endregion Public Properties

        #region Public Methods

        public IBlip? GetAt(ushort id)
            => RAGE.Entities.Blips.GetAt(id) as IBlip;

        #endregion Public Methods
    }
}
