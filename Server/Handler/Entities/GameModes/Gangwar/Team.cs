using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Entities.TeamSystem;

namespace TDS_Server.Handler.Entities.GameModes
{
    partial class Gangwar
    {
        public ITeam AttackerTeam => Lobby.Teams[2];
        public ITeam OwnerTeam => Lobby.Teams[1];
    }
}
