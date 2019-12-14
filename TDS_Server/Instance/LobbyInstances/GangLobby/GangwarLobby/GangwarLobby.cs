using TDS_Server.Enums;
using TDS_Server.Instance.GangTeam;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;

namespace TDS_Server.Instance.LobbyInstances
{
    partial class GangwarLobby : GangActionLobby
    {
        private TDSPlayer _attackLeader;
        private GangwarArea _gangwarArea;

        public GangwarLobby(TDSPlayer attacker, GangwarArea gangwarArea)
            : base(EGangActionType.Gangwar, attacker, gangwarArea.Owner!, "GW")
        {
            _attackLeader = attacker;
            _gangwarArea = gangwarArea;
        }

        protected override string ActionTypeName => "Gangwar";
        protected override string ActionTypeShort => "GW";
    }
}
