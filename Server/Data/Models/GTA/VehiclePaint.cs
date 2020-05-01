namespace TDS_Server.Data.Models.GTA
{
    public struct VehiclePaint
    {
        public int Type;
        public int Color;

        public VehiclePaint(int color)
            => (Type, Color) = (0, color);
        public VehiclePaint(int paintType, int paintColor)
            => (Type, Color) = (paintType, paintColor);

        public override bool Equals(object obj)
            => obj is VehiclePaint other && Type == other.Type && Color == other.Color;

        public override int GetHashCode()
            => base.GetHashCode();

        public static bool operator ==(VehiclePaint left, VehiclePaint right)
            => left.Equals(right);

        public static bool operator !=(VehiclePaint left, VehiclePaint right)
            => !left.Equals(right);
    }
}
