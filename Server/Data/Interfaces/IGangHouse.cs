﻿using TDS_Server.Database.Entity.GangEntities;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces
{
    public interface IGangHouse
    {
        GangHouses Entity { get; }
        Position3D Position { get; }
        float SpawnRotation { get; }
    }
}