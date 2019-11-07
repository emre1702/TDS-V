using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Instance.Player;

namespace TDS_Server.Instance.Lobby
{
    partial class GangwarLobby
    {
        public override bool CanStartPreparations(TDSPlayer player)
        {
            if (!base.CanStartPreparations(player))
                return false;

            return player.Gang.Entity.RankPermissions.StartGangwar == player.GangRank!.Rank;
        }
    }
}
