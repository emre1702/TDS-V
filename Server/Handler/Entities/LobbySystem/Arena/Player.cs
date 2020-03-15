using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities.Player;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Arena
    {
        public override async Task<bool> AddPlayer(TDSPlayer player, uint? teamindex = null)
        {
            if (CurrentGameMode?.CanJoinLobby(player, teamindex) == false)
                return false;

            if (!await base.AddPlayer(player, 0).ConfigureAwait(true))
                return false;
            SpectateOtherAllTeams(player);
            SendPlayerRoundInfoOnJoin(player);

            var teams = Teams.Select(t =>
                    new TeamChoiceMenuTeamData(t.Entity.Name, t.Entity.ColorR, t.Entity.ColorG, t.Entity.ColorB)
                )
                .ToList();

            NAPI.ClientEvent.TriggerClientEvent(player.Player, ToClientEvent.SyncTeamChoiceMenuData, Serializer.ToBrowser(teams), RoundSettings.MixTeamsAfterRound);

            CurrentGameMode?.AddPlayer(player, teamindex);

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
                case RoundStatus.NewMapChoose:
                case RoundStatus.Countdown:
                    if (Entity.LobbyRoundSettings.MixTeamsAfterRound)
                        BalanceCurrentTeams();
                    break;
                case RoundStatus.Round:
                    RoundCheckForEnoughAlive();
                    break;
            }
        }

        public override void SetPlayerTeam(TDSPlayer player, Team team)
        {
            base.SetPlayerTeam(player, team);

            if (CurrentRoundStatus == RoundStatus.Countdown || CurrentGameMode?.CanJoinDuringRound(player, team) == true)
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
                    if (CurrentRoundStatus != RoundStatus.None && CurrentGameMode?.CanEndRound(ERoundEndReason.NewPlayer) != false)
                        SetRoundStatus(RoundStatus.RoundEnd, ERoundEndReason.NewPlayer);
                    else
                        SetRoundStatus(RoundStatus.NewMapChoose);
                }
                else
                {
                    NAPI.ClientEvent.TriggerClientEvent(player.Player, ToClientEvent.PlayerSpectateMode);
                }
            }
        }

        private void SetPlayerReadyForRound(TDSPlayer player)
        {
            if (player.Player is null)
                return;

            if (player.Team != null && !player.Team.IsSpectator)
            {
                if (SpawnPlayer)
                {
                    Position4DDto? spawndata = GetMapRandomSpawnData(player.Team);
                    if (spawndata is null)
                        return;
                    NAPI.Player.SpawnPlayer(player.Player, spawndata.ToVector3(), spawndata.Rotation);
                }

                if (player.Team.SpectateablePlayers != null && !player.Team.SpectateablePlayers.Contains(player))
                    player.Team.SpectateablePlayers?.Add(player);
            }
            else
            {
                if (SpawnPlayer)
                    NAPI.Player.SpawnPlayer(player.Player, SpawnPoint, Entity.DefaultSpawnRotation);
            }


            RemoveAsSpectator(player);

            if (FreezePlayerOnCountdown)
                Workaround.FreezePlayer(player.Player, true);
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
            }, (uint)Entity.FightSettings.SpawnAgainAfterDeathMs);
        }

        private void StartRoundForPlayer(TDSPlayer player)
        {
            if (player.Player is null)
                return;
            NAPI.ClientEvent.TriggerClientEvent(player.Player, ToClientEvent.RoundStart, player.Team is null || player.Team.IsSpectator);
            if (player.Team != null && !player.Team.IsSpectator)
            {
                SetPlayerAlive(player);
                Workaround.FreezePlayer(player.Player, false);
            }
            player.LastHitter = null;
        }

        private void AddPlayerAsPlayer(TDSPlayer player, int teamIndex)
        {
            var team = Entity.LobbyRoundSettings.MixTeamsAfterRound ? GetTeamWithFewestPlayer() : Teams[teamIndex];
            SetPlayerTeam(player, team);
        }

        private void SendPlayerRoundInfoOnJoin(TDSPlayer player)
        {
            if (player.Player is null)
                return;

            if (_currentMap is { })
            {
                NAPI.ClientEvent.TriggerClientEvent(player.Player, ToClientEvent.MapChange, _currentMap.ClientSyncedDataJson);
            }

            SendPlayerAmountInFightInfo(player.Player);
            SyncMapVotingOnJoin(player.Player);
            CurrentGameMode?.SendPlayerRoundInfoOnJoin(player);

            switch (CurrentRoundStatus)
            {
                case RoundStatus.Countdown:
                    NAPI.ClientEvent.TriggerClientEvent(player.Player, ToClientEvent.CountdownStart, _nextRoundStatusTimer?.RemainingMsToExecute ?? 0);
                    break;

                case RoundStatus.Round:
                    NAPI.ClientEvent.TriggerClientEvent(player.Player, ToClientEvent.RoundStart, true, (int)(_nextRoundStatusTimer?.ElapsedMsSinceLastExecOrCreate ?? 0));
                    break;
            }
        }

        private void SendPlayerAmountInFightInfo(TDSPlayer player)
        {
            SyncedTeamPlayerAmountDto[] amounts = Teams.Skip(1).Select(t => t.SyncedTeamData).Select(t => t.AmountPlayers).ToArray();
            NAPI.ClientEvent.TriggerClientEvent(player, ToClientEvent.AmountInFightSync, Serializer.ToClient(amounts));
        }

        private void SetPlayerAlive(TDSPlayer player)
        {
            if (player.Team is null || player.Team.AlivePlayers is null)
                return;
            player.Lifes = (sbyte)(Entity.FightSettings?.AmountLifes ?? 0);
            player.Team.AlivePlayers.Add(player);
            var teamamountdata = player.Team.SyncedTeamData.AmountPlayers;
            ++teamamountdata.Amount;
            ++teamamountdata.AmountAlive;
        }

        private void SavePlayerRoundStats(TDSPlayer player)
        {
            if (!SavePlayerLobbyStats)
                return;
            if (player.LobbyStats is null)
                return;

            PlayerLobbyStats? to = player.LobbyStats;
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

            if (IsOfficial && from.Damage > 0)
            {
                if (from.Kills > 0)
                    player.AddToChallenge(EChallengeType.Kills, from.Kills);
                if (from.Assists > 0)
                    player.AddToChallenge(EChallengeType.Assists, from.Assists);
                player.AddToChallenge(EChallengeType.Damage, from.Damage);
                player.AddToChallenge(EChallengeType.RoundPlayed);
            }


            from.Clear();
        }

        private void RespawnPlayer(TDSPlayer player)
        {
            if (player.Player is null)
                return;

            SetPlayerReadyForRound(player);
            Workaround.FreezePlayer(player.Player, false);
            player.Player.TriggerEvent(ToClientEvent.PlayerRespawned);
        }
    }
}
