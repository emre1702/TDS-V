using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.LobbyInstances
{
    partial class GangActionLobby
    {
        public override Task<bool> AddPlayer(TDSPlayer player, uint? teamindex)
        {
            if (teamindex == 0 || teamindex is null)
                return base.AddPlayer(player, teamindex);

            bool isAttacker = AttackerTeam.Entity.Index == teamindex;
            if (!HasTeamFreePlace(isAttacker))
            {
                player.SendNotification(player.Language.GANGWAR_TEAM_ALREADY_FULL_INFO);
                return Task.FromResult(false);
            }
                

            return base.AddPlayer(player, teamindex);
        }

        public override void RemovePlayer(TDSPlayer player)
        {
            base.RemovePlayer(player);

            if (player == AttackLeader && AttackerTeam.Players.Count > 0)
            {
                var newAttackLeader = AttackerTeam.Players[0];
                SetAttackLeader(newAttackLeader);
            }
        }

        public void SetAttackLeader(TDSPlayer player)
        {
            AttackLeader = player;
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
