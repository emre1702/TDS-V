using TDS_Client.Data.Interfaces.ModAPI.Player;

namespace TDS_Client.Data.Models
{
    public struct TickNametagData
    {
        #region Public Fields

        public float Distance;
        public IPlayer Player;
        public float ScreenX;
        public float ScreenY;

        #endregion Public Fields

        #region Public Methods

        public static bool operator !=(TickNametagData left, TickNametagData right)
        {
            return !(left == right);
        }

        public static bool operator ==(TickNametagData left, TickNametagData right)
        {
            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is IPlayer otherPlayer))
                return false;
            return Player == otherPlayer;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion Public Methods
    }
}
