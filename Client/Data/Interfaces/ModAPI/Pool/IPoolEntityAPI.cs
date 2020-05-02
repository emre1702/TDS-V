using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.Entity;

namespace TDS_Client.Data.Interfaces.ModAPI.Pool
{
    public interface IPoolEntityAPI<T> where T : IEntityBase
    {
        T GetAtRemote(ushort handleValue);
        T GetAtHandle(int handle);

        List<T> All { get; }
        List<T> Streamed { get; }
    }
}
