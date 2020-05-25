using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces.ModAPI.Checkpoint;
using TDS_Server.Data.Interfaces.ModAPI.Pool;

namespace TDS_Server.RAGEAPI.Pool
{
    internal class PoolCheckpointsAPI : IPoolCheckpointsAPI
    {
        #region Public Constructors

        public PoolCheckpointsAPI()
        {
            RAGE.Entities.Checkpoints.CreateEntity = netHandle => new Checkpoint.Checkpoint(netHandle);
        }

        #endregion Public Constructors

        #region Public Properties

        public List<ICheckpoint> All => RAGE.Entities.Checkpoints.All.OfType<ICheckpoint>().ToList();
        public IEnumerable<ICheckpoint> AsEnumerable => RAGE.Entities.Checkpoints.AsEnumerable.OfType<ICheckpoint>();
        public int Count => RAGE.Entities.Checkpoints.Count;

        #endregion Public Properties

        #region Public Methods

        public ICheckpoint? GetAt(ushort id)
            => RAGE.Entities.Checkpoints.GetAt(id) as ICheckpoint;

        #endregion Public Methods
    }
}
