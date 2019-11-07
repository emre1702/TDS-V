using TDS_Server.Instance.GangTeam;
using TDS_Server.Instance.Player;

namespace TDS_Server.Instance.Lobby
{
    partial class GangActionLobby
    {
        public virtual bool CanStartPreparations(TDSPlayer player)
        {
            if (player.Gang == Gang.None)
                return false;

            if (player.GangRank is null)
                return false;

            if (player.Gang.Entity is null)
                return false;

            if (player.Gang.Entity.RankPermissions is null)
                return false;

            return true;
        }
    }
}
