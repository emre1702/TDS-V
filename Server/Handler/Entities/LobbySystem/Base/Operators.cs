using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Lobby
    {
        #region Public Methods

        public static bool operator !=(Lobby? lobby1, Lobby? lobby2)
        {
            if (lobby1 is null)
                return lobby2 is { };
            if (lobby2 is null)
                return true;
            return lobby1.Id != lobby2.Id;
        }

        public static bool operator ==(Lobby? lobby1, Lobby? lobby2)
        {
            if (lobby1 is null)
                return lobby2 is null;
            if (lobby2 is null)
                return false;
            return lobby1.Id == lobby2.Id;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;
            if (!obj.GetType().IsSubclassOf(typeof(Lobby)) && obj.GetType() != typeof(Lobby))
                return false;
            return Id == ((Lobby)obj).Id;
        }

        public bool Equals(ILobby? lobby)
        {
            if (lobby is null)
                return false;
            return Id == lobby.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion Public Methods
    }
}
