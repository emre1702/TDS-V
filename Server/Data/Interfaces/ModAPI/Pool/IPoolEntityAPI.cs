using System.Collections.Generic;

namespace TDS_Server.Data.Interfaces.ModAPI.Pool
{
    public interface IPoolEntityAPI<T> where T : IEntity
    {
        #region Public Properties

        public List<T> All { get; }
        public IEnumerable<T> AsEnumerable { get; }
        public int Count { get; }

        #endregion Public Properties

        #region Public Methods

        public T GetAt(ushort id);

        #endregion Public Methods
    }
}
