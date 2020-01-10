using TDS_Server.Instance.GangTeam;
using TDS_Server.Instance.Utility;

namespace TDS_Server.Instance.GameModes
{
    partial class Gangwar
    {
        public Team AttackerTeam => Lobby.Teams[1];
        public Team OwnerTeam => Lobby.Teams[2];
    }
}
