using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.GameModes
{
    partial class Gangwar
    {
        private TDSPlayer? _attackLeader;

        public override void AddPlayer(TDSPlayer player, uint? teamIndex)
        {
            base.AddPlayer(player, teamIndex);

            if (_attackLeader is null && teamIndex == AttackerTeam.Entity.Index) 
                SetAttackLeader(player);
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

        public override bool CanJoinLobby(TDSPlayer player, uint? teamIndex)
        {
            if (!teamIndex.HasValue)
                return true;

            bool isAttacker = AttackerTeam.Entity.Index == teamIndex;
            if (!HasTeamFreePlace(isAttacker))
            {
                player.SendNotification(player.Language.GANGWAR_TEAM_ALREADY_FULL_INFO);
                return false;
            }

            return true;
        }

        public override bool CanJoinDuringRound(TDSPlayer player, Team team)
        {
            if (Lobby.DmgSys.DamageDealtThisRound)
                return false;

            return true;
        }

        private void SetAttackLeader(TDSPlayer attackLeader)
        {
            _attackLeader = attackLeader;

            // Todo: Send him infos or open menu or whatever
        }

        private bool HasTeamFreePlace(bool isAttacker)
        {
            if (isAttacker)
            {
                return AttackerTeam.Players.Count < SettingsManager.ServerSettings.AmountPlayersAllowedInGangwarTeamBeforeCountCheck
                    || AttackerTeam.Players.Count < OwnerTeam.Players.Count + (SettingsManager.ServerSettings.GangwarAttackerCanBeMore ? 1 : 0);
            }
            else
            {
                return OwnerTeam.Players.Count < SettingsManager.ServerSettings.AmountPlayersAllowedInGangwarTeamBeforeCountCheck
                    || OwnerTeam.Players.Count < AttackerTeam.Players.Count + (SettingsManager.ServerSettings.GangwarOwnerCanBeMore ? 1 : 0);
            }
        }
    }
}
