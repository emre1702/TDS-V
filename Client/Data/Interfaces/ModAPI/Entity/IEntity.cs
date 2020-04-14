﻿using System;
using TDS_Client.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Entity
{
    public interface IEntity: IEquatable<IEntity>
    {
        int Handle { get; }
        ushort RemoteId { get; }

        Position3D Position { get; set; }
        uint Dimension { get; set; }
        bool IsNull { get; }
        bool Exists { get; }
        EntityType Type { get; }

        void Destroy();
    }
}