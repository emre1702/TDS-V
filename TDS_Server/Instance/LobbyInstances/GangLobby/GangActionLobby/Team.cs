using TDS_Server.Instance.Utility;

namespace TDS_Server.Instance.LobbyInstances
{
    partial class GangActionLobby
    {
        public Team SpectatorTeam => Teams[0];
        public Team AttackerTeam => Teams[1];
        public Team OwnerTeam => Teams[2];
    }    
}
