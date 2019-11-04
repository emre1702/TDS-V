using TDS_Server.Instance.GangTeam;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity.Gang;

namespace TDS_Server.Instance.Lobby
{
    partial class GangwarLobby : GangActionLobby
    {
        private TDSPlayer _attackLeader;

        public GangwarLobby(TDSPlayer attacker, int gangwarAreaId, Gang attackerGang, Gang ownerGang)
            : base(Enum.EGangActionType.Gangwar, attacker, attackerGang, ownerGang)
        {
            _attackLeader = attacker;
        }
    }
}
