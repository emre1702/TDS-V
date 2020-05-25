using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces.ModAPI.Marker;
using TDS_Server.Data.Interfaces.ModAPI.Pool;

namespace TDS_Server.RAGEAPI.Pool
{
    internal class PoolMarkersAPI : IPoolMarkersAPI
    {
        #region Public Constructors

        public PoolMarkersAPI()
        {
            RAGE.Entities.Markers.CreateEntity = netHandle => new Marker.Marker(netHandle);
        }

        #endregion Public Constructors

        #region Public Properties

        public List<IMarker> All => RAGE.Entities.Markers.All.OfType<IMarker>().ToList();
        public IEnumerable<IMarker> AsEnumerable => RAGE.Entities.Markers.AsEnumerable.OfType<IMarker>();
        public int Count => RAGE.Entities.Markers.Count;

        #endregion Public Properties

        #region Public Methods

        public IMarker? GetAt(ushort id)
            => RAGE.Entities.Markers.GetAt(id) as IMarker;

        #endregion Public Methods
    }
}
