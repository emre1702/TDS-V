using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server.Data.Abstracts.Entities.GTA
{
    public abstract class ITDSTextLabel : TextLabel
    {
        public ITDSTextLabel(NetHandle netHandle) : base(netHandle)
        {
        }
    }
}
