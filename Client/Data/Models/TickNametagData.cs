using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Data.Interfaces.ModAPI.Player;

namespace TDS_Client.Data.Models
{
    public struct TickNametagData
    {
        public IPlayer Player;
        public float ScreenX;
        public float ScreenY;
        public float Distance;

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

        public static bool operator ==(TickNametagData left, TickNametagData right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TickNametagData left, TickNametagData right)
        {
            return !(left == right);
        }
    }
}
