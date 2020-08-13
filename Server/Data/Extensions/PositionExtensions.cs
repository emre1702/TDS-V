using System;
using TDS_Server.Data.Models.Map.Creator;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Data.Utility;
using TDS_Shared.Core;

namespace TDS_Server.Data.Extensions
{
    public static class PositionExtensions
    {
        #region Public Methods

        public static PositionDto SwitchNamespace(this Position dto)
        {
            return new PositionDto { X = dto.X, Y = dto.Y, Z = dto.Z };
        }

        public static Position SwitchNamespace(this PositionDto dto)
        {
            return new Position { X = dto.X, Y = dto.Y, Z = dto.Z };
        }

        public static Position ToPosition(this Position4DDto pos)
        {
            return new Position(pos.X, pos.Y, pos.Z);
        }

        public static AltV.Net.Data.Position AddToZ(this AltV.Net.Data.Position pos, float z)
        {
            return new AltV.Net.Data.Position(pos.X, pos.Y, pos.Z + z);
        }

        public static AltV.Net.Data.Position Around(this AltV.Net.Data.Position pos, float around, bool considerZ = false)
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

        public static float Distance2D(this AltV.Net.Data.Position pos, AltV.Net.Data.Position otherPos)
        {
            var nX = pos.X - otherPos.X;
            var nY = pos.Y - otherPos.Y;

            return nX * nX + nY * nY;
        }

        #endregion Public Methods
    }
}
