namespace TDS_Server.Data.Models.GTA
{
    public struct VehiclePaint
    {
        #region Public Fields

        public int Color;
        public int Type;

        #endregion Public Fields

        #region Public Constructors

        public VehiclePaint(int color)
            => (Type, Color) = (0, color);

        public VehiclePaint(int paintType, int paintColor)
            => (Type, Color) = (paintType, paintColor);

        #endregion Public Constructors

        #region Public Methods

        public static bool operator !=(VehiclePaint left, VehiclePaint right)
            => !left.Equals(right);

        public static bool operator ==(VehiclePaint left, VehiclePaint right)
            => left.Equals(right);

        public override bool Equals(object obj)
                            => obj is VehiclePaint other && Type == other.Type && Color == other.Color;

        public override int GetHashCode()
            => base.GetHashCode();

        #endregion Public Methods
    }
}
