using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Data.Interfaces.ModAPI;

namespace TDS_Client.Core.Init
{
    public class Program
    {
        public Program(IModAPI modAPI)
        {
            Services.Initialize(modAPI);
        }
    }
}
