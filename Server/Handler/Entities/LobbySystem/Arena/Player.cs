using GTANetworkAPI;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Data.Models.CustomLobby;
using TDS_Server.Data.Models.Map.Creator;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Arena
    {
        #region Public Methods

        public override async Task<bool> AddPlayer(ITDSPlayer player, uint? teamindex = null)
        {
            if (CurrentGameMode?.CanJoinLobby(player, teamindex) == false)
                return false;

            if (!await base.AddPlayer(player, 0))
                return false;
            NAPI.Task.Run(() =>
            {
                var pos = _currentMap?.LimitInfo?.Center?.ToVector3();
                if (pos is { })
                    player.Position = pos.AddToZ(10);
                SendPlayerRoundInfoOnJoin(player);
                new TDSTimer(() => SpectateOtherAllTeams(player), 1000, 1);

                var teams = Teams.Select(t =>
                        new TeamChoiceMenuTeamData(t.Entity.Name, t.Entity.ColorR, t.Entity.ColorG, t.Entity.ColorB)
                    )
                    .ToList();

                player.TriggerEvent(ToClientEvent.SyncTeamChoiceMenuData, Serializer.ToBrowser(teams), RoundSettings.MixTeamsAfterRound);

                CurrentGameMode?.AddPlayer(player, teamindex);
            });

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
            var lifes = player.Lifes;
            await NAPI.Task.RunWait(() =>
            {
                if (lifes > 0)
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
            });

            await base.RemovePlayer(player);

            NAPI.Task.Run(() =>
            {
                switch (CurrentRoundStatus)
                {
                    case RoundStatus.NewMapChoose:
                    case RoundStatus.Countdown:
                        if (Entity.LobbyRoundSettings.MixTeamsAfterRound)
                            BalanceCurrentTeams();
                        break;

                    case RoundStatus.Round:
                        if (lifes > 0)
                            RoundCheckForEnoughAlive();
                        break;
                }
            });
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
                    player.TriggerEvent(ToClientEvent.PlayerSpectateMode);
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void AddPlayerAsPlayer(ITDSPlayer player, int teamIndex)
        {
            var team = Entity.LobbyRoundSettings.MixTeamsAfterRound ? GetTeamWithFewestPlayer() : Teams[teamIndex];
            SetPlayerTeam(player, team);
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

        private void RespawnPlayer(ITDSPlayer player)
        {
            SetPlayerReadyForRound(player);
            player.Freeze(false);
            player.TriggerEvent(ToClientEvent.PlayerRespawned);
            CurrentGameMode?.RespawnPlayer(player);
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

        private void SendPlayerAmountInFightInfo(ITDSPlayer player)
        {
            SyncedTeamPlayerAmountDto[] amounts = Teams.Skip(1).Select(t => t.SyncedTeamData).Select(t => t.AmountPlayers).ToArray();
            player.TriggerEvent(ToClientEvent.AmountInFightSync, Serializer.ToClient(amounts));
        }

        private void SendPlayerRoundInfoOnJoin(ITDSPlayer player)
        {
            if (_currentMap is { })
            {
                player.TriggerEvent(ToClientEvent.MapChange, _currentMap.ClientSyncedDataJson);
            }

            SendPlayerAmountInFightInfo(player);
            SyncMapVotingOnJoin(player);
            CurrentGameMode?.SendPlayerRoundInfoOnJoin(player);

            switch (CurrentRoundStatus)
            {
                case RoundStatus.Countdown:
                    player.TriggerEvent(ToClientEvent.CountdownStart, true, _nextRoundStatusTimer?.RemainingMsToExecute ?? 0);
                    break;

                case RoundStatus.Round:
                    player.TriggerEvent(ToClientEvent.RoundStart, true, (int)(_nextRoundStatusTimer?.ElapsedMsSinceLastExecOrCreate ?? 0));
                    break;
            }
        }

        private void SetPlayerAlive(ITDSPlayer player)
        {
            if (player.Team is null || player.Team.AlivePlayers is null)
                return;
            player.Lifes = AmountLifes;
            player.Team.AlivePlayers.Add(player);
            var teamamountdata = player.Team.SyncedTeamData.AmountPlayers;
            ++teamamountdata.Amount;
            ++teamamountdata.AmountAlive;
        }

        private void SetPlayerReadyForRound(ITDSPlayer player)
        {
            if (player.Team != null && !player.Team.IsSpectator)
            {
                if (SpawnPlayer)
                {
                    Position4DDto? spawndata = GetMapRandomSpawnData(player.Team);
                    if (spawndata is null)
                        return;
                    player.Spawn(spawndata.ToVector3(), spawndata.Rotation);
                }

                if (player.Team.SpectateablePlayers != null && !player.Team.SpectateablePlayers.Contains(player))
                    player.Team.SpectateablePlayers?.Add(player);

                player.Freeze(FreezePlayerOnCountdown);
                GivePlayerWeapons(player);
                RemoveAsSpectator(player);
            }
            else
            {
                if (SpawnPlayer)
                    player.Spawn(_currentMapSpectatorPosition?.ToVector3() ?? SpawnPoint, 0);

                player.Freeze(true);
                player.RemoveAllWeapons();
            }

            if (_removeSpectatorsTimer.ContainsKey(player))
                _removeSpectatorsTimer.Remove(player);
        }

        private void StartRoundForPlayer(ITDSPlayer player)
        {
            player.TriggerEvent(ToClientEvent.RoundStart, player.Team is null || player.Team.IsSpectator);
            if (player.Team?.IsSpectator == false)
            {
                SetPlayerAlive(player);
                player.Freeze(false);
            }
            player.LastHitter = null;
        }

        #endregion Private Methods
    }
}
