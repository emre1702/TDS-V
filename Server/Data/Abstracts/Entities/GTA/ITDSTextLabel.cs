using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDS.Server.Data.Abstracts.Entities.GTA
{
    public abstract class ITDSTextLabel : TextLabel
    {
        protected ITDSTextLabel(NetHandle netHandle) : base(netHandle)
        {
        }
    }
}
