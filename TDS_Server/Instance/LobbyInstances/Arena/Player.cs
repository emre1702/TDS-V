using GTANetworkAPI;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Common.Instance.Utility;
using TDS_Server.Dto;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Helper;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity.Player;
using TDS_Server.Dto.Map;
using TDS_Server.Dto.TeamChoiceMenu;
using TDS_Common.Manager.Utility;
using TDS_Server.Instance.Utility;
using TDS_Server.Enums;
using TDS_Common.Enum.Challenge;

namespace TDS_Server.Instance.LobbyInstances
{
    partial class Arena
    {
        public override async Task<bool> AddPlayer(TDSPlayer player, uint? teamindex = null)
        {
            if (!await base.AddPlayer(player, 0).ConfigureAwait(true))
                return false;
            SpectateOtherAllTeams(player);
            SendPlayerRoundInfoOnJoin(player);

            var teams = Teams.Select(t =>
                    new TeamChoiceMenuTeamData(t.Entity.Name, t.Entity.ColorR, t.Entity.ColorG, t.Entity.ColorB)
                )
                .ToList();

            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.SyncTeamChoiceMenuData, Serializer.ToBrowser(teams), RoundSettings.MixTeamsAfterRound);

            return true;
        }

        public void ChooseTeam(TDSPlayer player, int teamIndex)
        {
            player.CurrentRoundStats = new RoundStatsDto(player);

            if (teamIndex != 0)
            {
                SpectateOtherSameTeam(player);
                AddPlayerAsPlayer(player, teamIndex);
            }
        }

        public override void RemovePlayer(TDSPlayer player)
        {
            if (player.Lifes > 0)
            {
                RemovePlayerFromAlive(player);
                PlayerCantBeSpectatedAnymore(player);
                DmgSys.CheckLastHitter(player, out TDSPlayer? killercharacter);

                DeathInfoSync(player, killercharacter, (uint)WeaponHash.Unarmed);
            }
            else
            {
                SavePlayerRoundStats(player);
                RemoveAsSpectator(player);
            }
            CurrentGameMode?.RemovePlayer(player);
            base.RemovePlayer(player);

            switch (CurrentRoundStatus)
            {
                case ERoundStatus.NewMapChoose:
                case ERoundStatus.Countdown:
                    if (LobbyEntity.LobbyRoundSettings.MixTeamsAfterRound)
                        BalanceCurrentTeams();
                    break;
                case ERoundStatus.Round:
                    RoundCheckForEnoughAlive();
                    break;
            }
        }

        public override void SetPlayerTeam(TDSPlayer player, Team team)
        {
            base.SetPlayerTeam(player, team);

            if (CurrentRoundStatus == ERoundStatus.Countdown)
            {
                SetPlayerReadyForRound(player);
            }
            else
            {
                SpectateOtherSameTeam(player);
                int teamsinround = GetTeamAmountStillInRound();
                if (teamsinround < 2)
                {
                    CurrentRoundEndBecauseOfPlayer = player;
                    if (CurrentRoundStatus != ERoundStatus.None)
                        SetRoundStatus(ERoundStatus.RoundEnd, ERoundEndReason.NewPlayer);
                    else
                        SetRoundStatus(ERoundStatus.NewMapChoose);
                }
                else
                {
                    NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.PlayerSpectateMode);
                }
            }
        }

        private void SetPlayerReadyForRound(TDSPlayer player)
        {
            if (player.Client is null)
                return;

            if (player.Team != null && !player.Team.IsSpectator)
            {
                Position4DDto? spawndata = GetMapRandomSpawnData(player.Team);
                if (spawndata is null)
                    return;
                NAPI.Player.SpawnPlayer(player.Client, spawndata.ToVector3(), spawndata.Rotation);
                if (player.Team.SpectateablePlayers != null && !player.Team.SpectateablePlayers.Contains(player))
                    player.Team.SpectateablePlayers?.Add(player);
            }
            else
                NAPI.Player.SpawnPlayer(player.Client, SpawnPoint, LobbyEntity.DefaultSpawnRotation);

            RemoveAsSpectator(player);

            Workaround.FreezePlayer(player.Client, true);
            GivePlayerWeapons(player);

            if (_removeSpectatorsTimer.ContainsKey(player))
                _removeSpectatorsTimer.Remove(player);

            player.CurrentRoundStats?.Clear();
        }

        private void RemovePlayerFromAlive(TDSPlayer player)
        {
            if (player.Team != null)
            {
                player.Team.AlivePlayers?.Remove(player);
                --player.Team.SyncedTeamData.AmountPlayers.AmountAlive;
            }

            CurrentGameMode?.RemovePlayerFromAlive(player);

            _removeSpectatorsTimer[player] = new TDSTimer(() =>
            {
                PlayerCantBeSpectatedAnymore(player);
                SpectateOtherSameTeam(player);
            }, (uint)LobbyEntity.SpawnAgainAfterDeathMs);
        }

        private void StartRoundForPlayer(TDSPlayer player)
        {
            if (player.Client is null)
                return;
            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.RoundStart, player.Team is null || player.Team.IsSpectator);
            if (player.Team != null && !player.Team.IsSpectator)
            {
                SetPlayerAlive(player);
                Workaround.FreezePlayer(player.Client, false);
            }
            player.LastHitter = null;
        }

        private void AddPlayerAsPlayer(TDSPlayer player, int teamIndex)
        {
            var team = LobbyEntity.LobbyRoundSettings.MixTeamsAfterRound ? GetTeamWithFewestPlayer() : Teams[teamIndex];
            SetPlayerTeam(player, team);
        }

        private void SendPlayerRoundInfoOnJoin(TDSPlayer player)
        {
            if (player.Client is null)
                return;

            if (_currentMap != null)
            {
                NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.MapChange, _currentMap.Info.Name,
                    _currentMap.LimitInfo.EdgesJson, Serializer.ToClient(_currentMap.LimitInfo.Center));
            }

            SendPlayerAmountInFightInfo(player.Client);
            SyncMapVotingOnJoin(player.Client);
            CurrentGameMode?.SendPlayerRoundInfoOnJoin(player);

            switch (CurrentRoundStatus)
            {
                case ERoundStatus.Countdown:
                    NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.CountdownStart, _nextRoundStatusTimer?.RemainingMsToExecute ?? 0);
                    break;

                case ERoundStatus.Round:
                    NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.RoundStart, true, _nextRoundStatusTimer?.RemainingMsToExecute ?? 0);
                    break;
            }
        }

        private void SendPlayerAmountInFightInfo(Client player)
        {
            SyncedTeamPlayerAmountDto[] amounts = Teams.Skip(1).Select(t => t.SyncedTeamData).Select(t => t.AmountPlayers).ToArray();
            NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.AmountInFightSync, Serializer.ToClient(amounts));
        }

        private void SetPlayerAlive(TDSPlayer player)
        {
            if (player.Team is null || player.Team.AlivePlayers is null)
                return;
            player.Lifes = (sbyte)(LobbyEntity.AmountLifes ?? 0);
            player.Team.AlivePlayers.Add(player);
            var teamamountdata = player.Team.SyncedTeamData.AmountPlayers;
            ++teamamountdata.Amount;
            ++teamamountdata.AmountAlive;
        }

        private void SavePlayerRoundStats(TDSPlayer player)
        {
            if (!SavePlayerLobbyStats)
                return;
            if (player.CurrentLobbyStats is null)
                return;

            PlayerLobbyStats? to = player.CurrentLobbyStats;
            RoundStatsDto? from = player.CurrentRoundStats;
            if (to is null || from is null)
                return;
            to.Kills += from.Kills;
            to.Assists += from.Assists;
            to.Damage += from.Damage;
            to.TotalKills += from.Kills;
            to.TotalAssists += from.Assists;
            to.TotalDamage += from.Damage;

            ++to.TotalRounds;
            if (from.Kills > to.MostKillsInARound)
                to.MostKillsInARound = from.Kills;
            if (from.Damage > to.MostDamageInARound)
                to.MostDamageInARound = from.Damage;
            if (from.Assists > to.MostAssistsInARound)
                to.MostAssistsInARound = from.Assists;

            if (IsOfficial)
            {
                player.AddToChallenge(EChallengeType.Kills, from.Kills);
                player.AddToChallenge(EChallengeType.Assists, from.Assists);
                player.AddToChallenge(EChallengeType.Damage, from.Damage);
                player.AddToChallenge(EChallengeType.RoundPlayed);
            }
            

            from.Clear();
        }

        private void RespawnPlayer(TDSPlayer player)
        {
            if (player.Client is null)
                return;

            SetPlayerReadyForRound(player);
            Workaround.FreezePlayer(player.Client, false);
            player.Client.TriggerEvent(DToClientEvent.PlayerRespawned);
        }
    }
}