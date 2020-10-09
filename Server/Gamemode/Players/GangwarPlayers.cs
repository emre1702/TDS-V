using TDS_Server.Data.Interfaces.GamemodesSystem.Players;

namespace TDS_Server.GamemodesSystem.Players
{
    public class GangwarPlayers : BaseGamemodePlayers, IGangwarGamemodePlayers
    {
        //Todo: Do these for gangwar gang action
        /*
         * private ITDSPlayer? _attackLeader;
         *
         * public override void AddPlayer(ITDSPlayer player, uint? teamIndex)
        {
            base.AddPlayer(player, teamIndex);

            if (Lobby.IsGangActionLobby && _attackLeader is null && teamIndex == AttackerTeam.Entity.Index)
                SetAttackLeader(player);
        }

        // Check if this is needed first
         public override bool CanJoinLobby(ITDSPlayer player, uint? teamIndex)
        {
            if (!teamIndex.HasValue)
                return true;
            if (!Lobby.IsGangActionLobby)
                return true;

            bool isAttacker = AttackerTeam.Entity.Index == teamIndex;
            if (!HasTeamFreePlace(isAttacker))
            {
                player.SendNotification(player.Language.TEAM_ALREADY_FULL_INFO);
                return false;
            }

            return true;
        }

        public override void RemovePlayer(ITDSPlayer player)
        {
            base.RemovePlayer(player);

            if (player == _attackLeader && AttackerTeam.Players.Count > 0)
            {
                var newAttackLeader = AttackerTeam.Players.First();
                SetAttackLeader(newAttackLeader);
            }
        }

        private void SetAttackLeader(ITDSPlayer attackLeader)
        {
            _attackLeader = attackLeader;

            // Todo: Send him infos or open menu or whatever
        }

         */
    }
}
