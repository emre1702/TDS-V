using TDS_Server.Instance.GangTeam;
using TDS_Server.Instance.Utility;

namespace TDS_Server.Core.Instance.GameModes.Gangwar
{
    partial class Gangwar
    {
        public Team AttackerTeam => Lobby.Teams[2];
        public Team OwnerTeam => Lobby.Teams[1];
    }
}
