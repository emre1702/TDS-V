using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models;
using TDS_Server.Data.Models.Map.Creator;
using TDS_Shared.Core;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Arena
    {
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

            if (CurrentRoundStatus == RoundStatus.Countdown || CurrentGameMode?.CanJoinDuringRound(player, team) == true && CurrentRoundStatus == RoundStatus.Round)
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
                player.SetInvisible(true);
                SpectateOtherSameTeam(player, ignoreSource: true);
                if (player.Spectates is null || player.Spectates == player)
                    SpectateOtherAllTeams(player);
            }, (uint)Entity.FightSettings.SpawnAgainAfterDeathMs);
        }

        private void RespawnPlayer(ITDSPlayer player)
        {
            if (CurrentRoundStatus != RoundStatus.Round)
                return;
            SetPlayerReadyForRound(player);
            player.Freeze(false);
            player.TriggerEvent(ToClientEvent.PlayerRespawned);
            CurrentGameMode?.RespawnPlayer(player);
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
                player.SetInvisible(false);
            }
            else
            {
                if (SpawnPlayer)
                    player.Spawn(_currentMapSpectatorPosition?.ToVector3() ?? SpawnPoint, 0);

                player.Freeze(true);
                player.RemoveAllWeapons();
                player.SetInvisible(true);
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
    }
}
