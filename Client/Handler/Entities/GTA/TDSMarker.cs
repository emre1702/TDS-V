﻿using TDS_Client.Data.Abstracts.Entities.GTA;

namespace TDS_Client.Handler.Entities.GTA
{
    public class TDSMarker : ITDSMarker
    {
        public TDSMarker(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }
    }
}