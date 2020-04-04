using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Client.Data.Interfaces.ModAPI.Sync
{
    public interface IPoolAPI
    {
        IPoolPlayersAPI Players { get; }
    }
}
