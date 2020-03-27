using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server.Data.Interfaces.ModAPI.Server
{
    public interface IServerAPI
    {
        string GetName();
        int GetPort();
    }
}
