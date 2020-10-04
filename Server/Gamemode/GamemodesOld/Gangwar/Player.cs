/*using MoreLinq;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Utility;
using TDS_Shared.Default;

namespace TDS_Server.LobbySystem.Gamemodes
{
    partial class Gangwar
    {
        private ITDSPlayer? _attackLeader;
        private ITDSPlayer? _playerForcedAtTarget;

        public override void AddPlayer(ITDSPlayer player, uint? teamIndex)
        {
            base.AddPlayer(player, teamIndex);

            if (Lobby.IsGangActionLobby && _attackLeader is null && teamIndex == AttackerTeam.Entity.Index)
                SetAttackLeader(player);
        }

        // In GangAction lobby this isn't checked //
        public override bool CanJoinDuringRound(ITDSPlayer player, ITeam team) => false;

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

        public override void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer)
        {
            base.OnPlayerDeath(player, killer);

            if (player == _playerForcedAtTarget)
            {
                var nextTargetMan = GetNextTargetMan();
                SetTargetMan(nextTargetMan);
            }
        }

        public override void RemovePlayer(ITDSPlayer player)
        {
            base.RemovePlayer(player);

            if (player == _attackLeader && AttackerTeam.Players.Count > 0)
            {
                var newAttackLeader = AttackerTeam.Players.First();
                SetAttackLeader(newAttackLeader);
            }

            if (player == _playerForcedAtTarget)
            {
                var nextTargetMan = GetNextTargetMan();
                SetTargetMan(nextTargetMan);
            }
        }

        private ITDSPlayer? GetNextTargetMan()
        {
            if (TargetObject is null)
                return null;

            if (AttackerTeam.Players.Count == 0)
                return null;

            if (Lobby.Roun.CurrentRoundStatus == RoundStatus.Round && AttackerTeam.AlivePlayers!.Count == 0)
                return null;

            if (Lobby.CurrentRoundStatus != RoundStatus.Round)
                return SharedUtils.GetRandom(AttackerTeam.Players);

            return AttackerTeam.Players.MinBy(p => p.Position.DistanceTo(TargetObject.Position)).FirstOrDefault();
        }

        private void SetAttackLeader(ITDSPlayer attackLeader)
        {
            _attackLeader = attackLeader;

            // Todo: Send him infos or open menu or whatever
        }

        private void SetTargetMan(ITDSPlayer? player)
        {
            if (_playerForcedAtTarget is { })
                _playerForcedAtTarget.TriggerEvent(ToClientEvent.RemoveForceStayAtPosition);

            _playerForcedAtTarget = player;

            if (_playerForcedAtTarget is null)
                return;

            AttackerTeam.FuncIterate(player =>
            {
                player.SendNotification(string.Format(player.Language.TARGET_PLAYER_DEFEND_INFO, _playerForcedAtTarget.DisplayName));
            });

            _playerForcedAtTarget.TriggerEvent(ToClientEvent.SetForceStayAtPosition,
                Serializer.ToClient(TargetObject!.Position),
                SettingsHandler.ServerSettings.GangwarTargetRadius,
                MapLimitType.KillAfterTime,
                SettingsHandler.ServerSettings.GangwarTargetWithoutAttackerMaxSeconds);
        }
    }
}
*/
