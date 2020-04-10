using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.Object;

namespace TDS_Client.Data.Interfaces.ModAPI.Pool
{
    public interface IPoolObjectsAPI : IPoolEntityAPI<IObject>
    {
        IEnumerable<IObject> All { get; }
    }
}
