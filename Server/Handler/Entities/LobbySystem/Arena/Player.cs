using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Data.Models.CustomLobby;
using TDS_Server.Data.Models.Map.Creator;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities.TeamSystem;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Arena
    {
        public override async Task<bool> AddPlayer(ITDSPlayer player, uint? teamindex = null)
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

            player.SendEvent(ToClientEvent.SyncTeamChoiceMenuData, Serializer.ToBrowser(teams), RoundSettings.MixTeamsAfterRound);

            CurrentGameMode?.AddPlayer(player, teamindex);

            return true;
        }

        public void ChooseTeam(ITDSPlayer player, int teamIndex)
        {
            player.CurrentRoundStats = new RoundStatsDto(player);

            if (teamIndex != 0)
            {
                SpectateOtherSameTeam(player);
                AddPlayerAsPlayer(player, teamIndex);
            }
        }

        public override async Task RemovePlayer(ITDSPlayer player)
        {
            if (player.Lifes > 0)
            {
                RemovePlayerFromAlive(player);
                PlayerCantBeSpectatedAnymore(player);
                DmgSys.CheckLastHitter(player, out ITDSPlayer? killercharacter);

                DeathInfoSync(player, killercharacter, (uint)WeaponHash.Unarmed);
            }
            else
            {
                SavePlayerRoundStats(player);
                RemoveAsSpectator(player);
            }
            CurrentGameMode?.RemovePlayer(player);
            await base.RemovePlayer(player);

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

        public override void SetPlayerTeam(ITDSPlayer player, ITeam? team)
        {
            base.SetPlayerTeam(player, team);

            // That means the player left the lobby
            if (team is null)
                return;

            if (CurrentRoundStatus == RoundStatus.Countdown || CurrentGameMode?.CanJoinDuringRound(player, team) == true)
            {
                SetPlayerReadyForRound(player);
            }
            else
            {
                SpectateOtherSameTeam(player);
                if (!team.IsSpectator && GetTeamAmountStillInRound() < 2)
                {
                    CurrentRoundEndBecauseOfPlayer = player;
                    if (CurrentRoundStatus != RoundStatus.None && CurrentGameMode?.CanEndRound(RoundEndReason.NewPlayer) != false)
                        SetRoundStatus(RoundStatus.RoundEnd, RoundEndReason.NewPlayer);
                    else
                        SetRoundStatus(RoundStatus.NewMapChoose);
                }                
                else
                {
                    player.SendEvent(ToClientEvent.PlayerSpectateMode);
                }
            }
        }

        private void SetPlayerReadyForRound(ITDSPlayer player)
        {
            if (player.ModPlayer is null)
                return;

            if (player.Team != null && !player.Team.IsSpectator)
            {
                if (SpawnPlayer)
                {
                    Position4DDto? spawndata = GetMapRandomSpawnData(player.Team);
                    if (spawndata is null)
                        return;
                    player.Spawn(spawndata.To3D(), spawndata.Rotation);
                }

                if (player.Team.SpectateablePlayers != null && !player.Team.SpectateablePlayers.Contains(player))
                    player.Team.SpectateablePlayers?.Add(player);

                player.ModPlayer.Freeze(FreezePlayerOnCountdown);
                GivePlayerWeapons(player);
            }
            else
            {
                if (SpawnPlayer)
                    player.Spawn(SpawnPoint, Entity.DefaultSpawnRotation);

                player.ModPlayer.Freeze(true);
                player.ModPlayer.RemoveAllWeapons();
            }

            RemoveAsSpectator(player);

            if (_removeSpectatorsTimer.ContainsKey(player))
                _removeSpectatorsTimer.Remove(player);

            player.CurrentRoundStats?.Clear();
        }

        private void RemovePlayerFromAlive(ITDSPlayer player)
        {
            if (player.Team != null)
            {
                player.Team.RemoveAlivePlayer(player);
            }

            CurrentGameMode?.RemovePlayerFromAlive(player);

            _removeSpectatorsTimer[player] = new TDSTimer(() =>
            {
                PlayerCantBeSpectatedAnymore(player);
                SpectateOtherSameTeam(player);
            }, (uint)Entity.FightSettings.SpawnAgainAfterDeathMs);
        }

        private void StartRoundForPlayer(ITDSPlayer player)
        {
            if (player.ModPlayer is null)
                return;
            player.SendEvent(ToClientEvent.RoundStart, player.Team is null || player.Team.IsSpectator);
            if (player.Team != null && !player.Team.IsSpectator)
            {
                SetPlayerAlive(player);
                player.ModPlayer.Freeze(false);
            }
            player.LastHitter = null;
        }

        private void AddPlayerAsPlayer(ITDSPlayer player, int teamIndex)
        {
            var team = Entity.LobbyRoundSettings.MixTeamsAfterRound ? GetTeamWithFewestPlayer() : Teams[teamIndex];
            SetPlayerTeam(player, team);
        }

        private void SendPlayerRoundInfoOnJoin(ITDSPlayer player)
        {
            if (player.ModPlayer is null)
                return;

            if (_currentMap is { })
            {
                player.SendEvent(ToClientEvent.MapChange, _currentMap.ClientSyncedDataJson);
            }

            SendPlayerAmountInFightInfo(player);
            SyncMapVotingOnJoin(player);
            CurrentGameMode?.SendPlayerRoundInfoOnJoin(player);

            switch (CurrentRoundStatus)
            {
                case RoundStatus.Countdown:
                    player.SendEvent(ToClientEvent.CountdownStart, true, _nextRoundStatusTimer?.RemainingMsToExecute ?? 0);
                    break;

                case RoundStatus.Round:
                    player.SendEvent(ToClientEvent.RoundStart, true, (int)(_nextRoundStatusTimer?.ElapsedMsSinceLastExecOrCreate ?? 0));
                    break;
            }
        }

        private void SendPlayerAmountInFightInfo(ITDSPlayer player)
        {
            SyncedTeamPlayerAmountDto[] amounts = Teams.Skip(1).Select(t => t.SyncedTeamData).Select(t => t.AmountPlayers).ToArray();
            player.SendEvent(ToClientEvent.AmountInFightSync, Serializer.ToClient(amounts));
        }

        private void SetPlayerAlive(ITDSPlayer player)
        {
            if (player.Team is null || player.Team.AlivePlayers is null)
                return;
            player.Lifes = (sbyte)(Entity.FightSettings?.AmountLifes ?? 0);
            player.Team.AlivePlayers.Add(player);
            var teamamountdata = player.Team.SyncedTeamData.AmountPlayers;
            ++teamamountdata.Amount;
            ++teamamountdata.AmountAlive;
        }

        private void SavePlayerRoundStats(ITDSPlayer player)
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
                    player.AddToChallenge(ChallengeType.Kills, from.Kills);
                if (from.Assists > 0)
                    player.AddToChallenge(ChallengeType.Assists, from.Assists);
                player.AddToChallenge(ChallengeType.Damage, from.Damage);
                player.AddToChallenge(ChallengeType.RoundPlayed);
            }


            from.Clear();
        }

        private void RespawnPlayer(ITDSPlayer player)
        {
            if (player.ModPlayer is null)
                return;

            SetPlayerReadyForRound(player);
            player.ModPlayer.Freeze(false);
            player.SendEvent(ToClientEvent.PlayerRespawned);
        }
    }
}
