using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server.Instance.Lobby
{
    partial class Lobby
    {
        public static bool operator ==(Lobby? lobby1, Lobby? lobby2)
        {
            if (lobby1 is null)
                return lobby2 is null;
            if (lobby2 is null)
                return false;
            return lobby1.Id == lobby2.Id;
        }

        public static bool operator !=(Lobby? lobby1, Lobby? lobby2)
        {
            if (lobby1 is null)
                return !(lobby2 is null);
            if (lobby2 is null)
                return true;
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
