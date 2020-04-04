using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Data.Interfaces.ModAPI.Entity;

namespace TDS_Client.Data.Interfaces.ModAPI.Sync
{
    public interface IPoolEntityAPI<T> where T : IEntity
    {
        T GetAtRemote(ushort handleValue);
    }
}
