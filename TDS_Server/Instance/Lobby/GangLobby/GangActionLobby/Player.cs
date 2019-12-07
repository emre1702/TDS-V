using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDS_Server.Instance.Player;

namespace TDS_Server.Instance.Lobby
{
    partial class GangActionLobby
    {
        public override Task<bool> AddPlayer(TDSPlayer player, uint? teamindex)
        {
            return base.AddPlayer(player, teamindex);
        }

        public override void RemovePlayer(TDSPlayer player)
        {
            base.RemovePlayer(player);

            if (player == _attackLeader && AttackerTeam.Players.Count > 0)
            {
                var newAttackLeader = AttackerTeam.Players[0];
                SetAttackLeader(newAttackLeader);
            }
        }

        public void SetAttackLeader(TDSPlayer player)
        {
            _attackLeader = player;
            // Todo: Send him infos or open menu or whatever
        }
    }
}
