﻿using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Handler.Entities.GTA.TextLabels
{
    public class TDSTextLabel : ITDSTextLabel
    {
        public TDSTextLabel(NetHandle netHandle) : base(netHandle)
        {
        }
    }
}
