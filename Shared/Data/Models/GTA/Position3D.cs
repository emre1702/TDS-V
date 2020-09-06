using Newtonsoft.Json;
using System;
using TDS_Shared.Data.Attributes;
using TDS_Shared.Data.Extensions;
using TDS_Shared.Data.Models.Map.Creator;
using TDS_Shared.Data.Utility;

namespace TDS_Shared.Data.Models.GTA
{
    [TDSCommandArgLength(3)]
    public class Position3D
    {
        #region Public Constructors

        public Position3D()
        {
        }

        public Position3D(Position3D pos)
            => (X, Y, Z) = (pos.X, pos.Y, pos.Z);

        public Position3D(MapCreatorPosition pos)
            => (X, Y, Z) = (pos.PosX, pos.PosY, pos.PosZ);

        public Position3D(float x, float y, float z)
            => (X, Y, Z) = (x, y, z);

        public Position3D(double x, double y, double z)
           => (X, Y, Z) = ((float)x, (float)y, (float)z);

        public Position3D(int x, int y, int z)
           => (X, Y, Z) = (x, y, z);

        #endregion Public Constructors

        #region Public Properties

        [JsonIgnore]
        public Position3D Normalized
        {
            get
            {
                var len = Length();

                return new Position3D(X / len, Y / len, Z / len);
            }
        }

        [JsonProperty("0")]
        public float X { get; set; }

        [JsonProperty("1")]
        public float Y { get; set; }

        [JsonProperty("2")]
        public float Z { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static float Distance(Position3D a, Position3D b)
        {
            return a.DistanceTo(b);
        }

        public static float DistanceSquared(Position3D a, Position3D b)
        {
            return a.DistanceToSquared(b);
        }

        public static Position3D GetPos(MapCreatorPosition pos)
            => new Position3D(pos.PosX, pos.PosY, pos.PosZ);

        public static Position3D GetRot(MapCreatorPosition pos)
            => new Position3D(pos.RotX, pos.RotY, pos.RotZ);

        public static Position3D Lerp(Position3D start, Position3D end, float n)
        {
            return new Position3D()
            {
                X = start.X + (end.X - start.X) * n,
                Y = start.Y + (end.Y - start.Y) * n,
                Z = start.Z + (end.Z - start.Z) * n,
            };
        }

        public static Position3D operator -(Position3D left, Position3D right)
        {
            if (left is null || right is null) return new Position3D();
            return new Position3D(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static bool operator !=(Position3D left, Position3D right)
        {
            if (left is null && right is null) return false;
            if (left is null || right is null) return true;
            return left.X != right.X || left.Y != right.Y || left.Z != right.Z;
        }

        public static Position3D operator *(Position3D left, float right)
        {
            if (left is null) return new Position3D();
            return new Position3D(left.X * right, left.Y * right, left.Z * right);
        }

        public static Position3D operator /(Position3D left, float right)
        {
            if (left is null) return new Position3D();
            return new Position3D(left.X / right, left.Y / right, left.Z / right);
        }

        public static Position3D operator +(Position3D left, Position3D right)
        {
            if (left is null || right is null) return new Position3D();
            return new Position3D(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static bool operator ==(Position3D left, Position3D right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
        }

        public static Position3D RandomXY()
        {
            var v = new Position3D();
            double radian = SharedUtils.Rnd.NextDouble() * 2 * Math.PI;

            v.X = (float)Math.Cos(radian);
            v.Y = (float)Math.Sin(radian);
            v.Normalize();

            return v;
        }

        public Position3D Add(Position3D right)
        {
            return this + right;
        }

        public Position3D AddToZ(float z)
            => new Position3D(X, Y, Z + z);

        public Position3D Around(float around, bool considerZ = false)
        {
            float addToX = SharedUtils.Rnd.NextFloat(-around, around);
            X += addToX;
            around -= Math.Abs(addToX);

            if (around == 0)
                return this;

            if (!considerZ)
            {
                Y += SharedUtils.GetRandom(true, false) ? around : -around;
                return this;
            }

            float addToY = SharedUtils.Rnd.NextFloat(-around, around);
            Y += addToY;
            around -= addToY;

            if (around == 0)
                return this;

            Z += SharedUtils.GetRandom(true, false) ? around : -around;

            return this;
        }

        public Position3D Around(float distance)
        {
            return this + RandomXY() * distance;
        }

        public float DistanceTo(Position3D right)
        {
            if (right is null) return 0f;
            return (float)Math.Sqrt(DistanceToSquared(right));
        }

        public float DistanceTo2D(Position3D right)
        {
            if (right is null) return 0f;
            return (float)Math.Sqrt(DistanceToSquared2D(right));
        }

        public float DistanceToSquared(Position3D right)
        {
            if (right is null) return 0f;

            var nX = X - right.X;
            var nY = Y - right.Y;
            var nZ = Z - right.Z;

            return nX * nX + nY * nY + nZ * nZ;
        }

        public float DistanceToSquared2D(Position3D right)
        {
            if (right is null) return 0f;

            var nX = X - right.X;
            var nY = Y - right.Y;

            return nX * nX + nY * nY;
        }

        public Position3D Divide(float right)
        {
            return this / right;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is null)
            {
                return false;
            }

            if (!(obj is Position3D otherPos))
                return false;

            return X == otherPos.X && Y == otherPos.Y && Z == otherPos.Z;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this);
        }

        public float Length()
        {
            return (float)Math.Sqrt(LengthSquared());
        }

        public float LengthSquared()
        {
            return X * X + Y * Y + Z * Z;
        }

        public Position3D Multiply(float right)
        {
            return this * right;
        }

        public void Normalize()
        {
            var len = Length();

            X /= len;
            Y /= len;
            Z /= len;
        }

        public Position3D Subtract(Position3D right)
        {
            return this - right;
        }

        public override string ToString()
        {
            return string.Format("X: {0} Y: {1} Z: {2}", X, Y, Z);
        }

        #endregion Public Methods
    }
}
