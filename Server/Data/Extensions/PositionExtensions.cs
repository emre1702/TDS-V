using AltV.Net.Data;
using System;
using TDS_Server.Data.Models.Map.Creator;
using TDS_Shared.Core;
using TDS_Shared.Data.Utility;

namespace TDS_Server.Data.Extensions
{
    public static class PositionExtensions
    {
        #region Public Methods

        public static PositionDto SwitchNamespace(this Position dto)
        {
            return new PositionDto { X = dto.X, Y = dto.Y, Z = dto.Z };
        }

        public static Position ToAltV(this PositionDto dto)
        {
            return new Position(dto.X, dto.Y, dto.Z);
        }

        public static Position ToAltV(this Position4DDto pos)
        {
            return new Position(pos.X, pos.Y, pos.Z);
        }

        public static Position AddToZ(this Position pos, float z)
        {
            return new Position(pos.X, pos.Y, pos.Z + z);
        }

        public static Position Around(this Position pos, float around, bool considerZ = false)
        {
            float addToX = SharedUtils.Rnd.NextFloat(-around, around);
            pos.X += addToX;
            around -= Math.Abs(addToX);

            if (around == 0)
                return pos;

            if (!considerZ)
            {
                pos.Y += SharedUtils.GetRandom(true, false) ? around : -around;
                return pos;
            }

            float addToY = SharedUtils.Rnd.NextFloat(-around, around);
            pos.Y += addToY;
            around -= addToY;

            if (around == 0)
                return pos;

            pos.Z += SharedUtils.GetRandom(true, false) ? around : -around;

            return pos;
        }

        public static float Distance(this Position pos, Position otherPos)
        {
            var nX = pos.X - otherPos.X;
            var nY = pos.Y - otherPos.Y;
            var nZ = pos.Z - otherPos.Z;

            return (float)Math.Sqrt(nX * nX + nY * nY + nZ * nZ);
        }

        public static float Distance2D(this Position pos, Position otherPos)
        {
            var nX = pos.X - otherPos.X;
            var nY = pos.Y - otherPos.Y;

            return (float)Math.Sqrt(nX * nX + nY * nY);
        }

        #endregion Public Methods
    }
}
