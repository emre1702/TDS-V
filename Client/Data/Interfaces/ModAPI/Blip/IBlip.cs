﻿using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Blip
{
    public interface IBlip : IEntity
    {
        Position3D Rotation { set; }
    }
}
