﻿using System;
using System.Collections.Generic;
using System.Text;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.MapObject
{
    public interface IMapObjectAPI
    {
        IMapObject Create(uint hash, Position3D position, Position3D rotation, int alpha = 255, uint dimension = 0);
        void PlaceObjectOnGroundProperly(int handle);
    }
}