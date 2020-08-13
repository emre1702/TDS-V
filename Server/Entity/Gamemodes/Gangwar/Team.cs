using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Entity.Gamemodes.Gangwar
{
    partial class Gangwar
    {
        #region Public Properties

        public ITeam AttackerTeam => Lobby.Teams[2];
        public ITeam OwnerTeam => Lobby.Teams[1];

        #endregion Public Properties
    }
}
