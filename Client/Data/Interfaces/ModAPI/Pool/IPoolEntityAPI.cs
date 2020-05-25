using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.Entity;

namespace TDS_Client.Data.Interfaces.ModAPI.Pool
{
    public interface IPoolEntityAPI<T> where T : IEntityBase
    {
        #region Public Properties

        List<T> All { get; }

        List<T> Streamed { get; }

        #endregion Public Properties

        #region Public Methods

        T GetAtHandle(int handle);

        T GetAtRemote(ushort handleValue);

        #endregion Public Methods
    }
}
