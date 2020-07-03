using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.Gamemodes
{
    partial class Gangwar
    {
        #region Public Properties

        public ITeam AttackerTeam => Lobby.Teams[2];
        public ITeam OwnerTeam => Lobby.Teams[1];

        #endregion Public Properties
    }
}
