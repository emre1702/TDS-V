﻿using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Handler.Entities.GTA.Objects
{
    public partial class TDSObject : ITDSObject
    {
        private readonly WorkaroundsHandler _workaroundsHandler;

        public TDSObject(NetHandle netHandle, WorkaroundsHandler workaroundsHandler) : base(netHandle)
        {
            _workaroundsHandler = workaroundsHandler;
        }
    }
}
