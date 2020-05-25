﻿using TDS_Server.Data.Models.Map.Creator;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Extensions
{
    public static class PositionExtensions
    {
        #region Public Methods

        public static Position3DDto SwitchNamespace(this Position3D dto)
        {
            return new Position3DDto { X = dto.X, Y = dto.Y, Z = dto.Z };
        }

        public static Position3D SwitchNamespace(this Position3DDto dto)
        {
            return new Position3D { X = dto.X, Y = dto.Y, Z = dto.Z };
        }

        public static Position3D ToPosition3D(this Position4DDto pos)
        {
            return new Position3D(pos.X, pos.Y, pos.Z);
        }

        #endregion Public Methods
    }
}
