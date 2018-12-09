using System;
using System.Collections.Generic;
using System.Text;

namespace TDS.Instance.Lobby
{
    partial class Lobby
    {
        public static bool operator ==(Lobby lobby1, Lobby lobby2)
        {
            return lobby1.Id == lobby2.Id;
        }

        public static bool operator !=(Lobby lobby1, Lobby lobby2)
        {
            return lobby1.Id != lobby2.Id;
        }

        public override bool Equals(object obj)
        {
            if (!obj.GetType().IsSubclassOf(typeof(Lobby)) && obj.GetType() != typeof(Lobby))
                return false;
            return Id == ((Lobby)obj).Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
