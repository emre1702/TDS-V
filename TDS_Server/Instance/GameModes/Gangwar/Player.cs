using GTANetworkAPI;
using MoreLinq;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto.Map;
using TDS_Server.Enums;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.GameModes
{
    partial class Gangwar
    {
        private TDSPlayer? _attackLeader;
        private TDSPlayer? _playerForcedAtTarget;

        public override void AddPlayer(TDSPlayer player, uint? teamIndex)
        {
            base.AddPlayer(player, teamIndex);

            if (Lobby.IsGangActionLobby && _attackLeader is null && teamIndex == AttackerTeam.Entity.Index) 
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

            if (player == _playerForcedAtTarget)
            {
                var nextTargetMan = GetNextTargetMan();
                SetTargetMan(nextTargetMan);
            }
        }

        public override void OnPlayerDeath(TDSPlayer player, TDSPlayer killer)
        {
            base.OnPlayerDeath(player, killer);

            if (player == _playerForcedAtTarget)
            {
                var nextTargetMan = GetNextTargetMan();
                SetTargetMan(nextTargetMan);
            }
        }

        public override bool CanJoinLobby(TDSPlayer player, uint? teamIndex)
        {
            if (!teamIndex.HasValue)
                return true;
            if (!Lobby.IsGangActionLobby)
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
            if (!Lobby.IsGangActionLobby)
                return false;
            if (Lobby.DmgSys.DamageDealtThisRound)
                return false;

            return true;
        }

        private void SetAttackLeader(TDSPlayer attackLeader)
        {
            _attackLeader = attackLeader;

            // Todo: Send him infos or open menu or whatever
        }

        private TDSPlayer? GetNextTargetMan()
        {
            if (TargetObject is null)
                return null;

            if (AttackerTeam.Players.Count == 0)
                return null;

            if (Lobby.CurrentRoundStatus == ERoundStatus.Round && AttackerTeam.AlivePlayers!.Count == 0)
                return null;

            if (Lobby.CurrentRoundStatus != ERoundStatus.Round)
                return AttackerTeam.Players[CommonUtils.Rnd.Next(AttackerTeam.Players.Count)];

            return AttackerTeam.Players.MinBy(p => p.Player!.Position.DistanceTo(TargetObject.Position)).FirstOrDefault();
        }

        private void SetTargetMan(TDSPlayer? player)
        {
            if (_playerForcedAtTarget is { })
                NAPI.ClientEvent.TriggerClientEvent(_playerForcedAtTarget.Player, DToClientEvent.RemoveForceStayAtPosition);

            _playerForcedAtTarget = player;

            if (_playerForcedAtTarget is null)
                return;

            AttackerTeam.FuncIterate((player, team) =>
            {
                player.SendNotification(string.Format(player.Language.TARGET_PLAYER_DEFEND_INFO, _playerForcedAtTarget.DisplayName));
            });

            NAPI.ClientEvent.TriggerClientEvent(_playerForcedAtTarget.Player, DToClientEvent.SetForceStayAtPosition,
                Serializer.ToClient(new Position3DDto(TargetObject!.Position)),
                SettingsManager.ServerSettings.GangwarTargetRadius,
                EMapLimitType.KillAfterTime,
                SettingsManager.ServerSettings.GangwarTargetWithoutAttackerMaxSeconds);
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
