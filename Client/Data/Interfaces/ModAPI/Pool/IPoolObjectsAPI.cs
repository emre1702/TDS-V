﻿using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI.MapObject;

namespace TDS_Client.Data.Interfaces.ModAPI.Pool
{
    public interface IPoolObjectsAPI : IPoolEntityAPI<IMapObject>
    {
        IEnumerable<IMapObject> All { get; }
    }
}
