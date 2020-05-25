using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces.ModAPI.Pool;
using TDS_Server.Data.Interfaces.ModAPI.TextLabel;

namespace TDS_Server.RAGEAPI.Pool
{
    internal class PoolTextLabelsAPI : IPoolTextLabelsAPI
    {
        #region Public Constructors

        public PoolTextLabelsAPI()
        {
            RAGE.Entities.TextLabels.CreateEntity = netHandle => new TextLabel.TextLabel(netHandle);
        }

        #endregion Public Constructors

        #region Public Properties

        public List<ITextLabel> All => RAGE.Entities.TextLabels.All.OfType<ITextLabel>().ToList();
        public IEnumerable<ITextLabel> AsEnumerable => RAGE.Entities.TextLabels.AsEnumerable.OfType<ITextLabel>();
        public int Count => RAGE.Entities.TextLabels.Count;

        #endregion Public Properties

        #region Public Methods

        public ITextLabel? GetAt(ushort id)
            => RAGE.Entities.TextLabels.GetAt(id) as ITextLabel;

        #endregion Public Methods
    }
}
