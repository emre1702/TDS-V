﻿using GTANetworkAPI;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.Players;
using TDS.Server.Data.Models;
using TDS.Server.Data.Models.Map;
using TDS.Server.Handler.Extensions;
using TDS.Server.LobbySystem.RoundsHandlers.Datas.RoundStates;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums.Challenge;
using TDS.Shared.Default;

namespace TDS.Server.LobbySystem.Players
{
    public class RoundFightLobbyPlayers : FightLobbyPlayers, IRoundFightLobbyPlayers
    {
        protected new IRoundFightLobbyEventsHandler Events => (IRoundFightLobbyEventsHandler)base.Events;
        protected new IRoundFightLobby Lobby => (IRoundFightLobby)base.Lobby;

        protected bool ShouldRewardPlayerForRound => Lobby.Entity.LobbyRewards is { } rewards &&
            (rewards.MoneyPerAssist != 0 || rewards.MoneyPerDamage != 0 || rewards.MoneyPerKill != 0);

        public RoundFightLobbyPlayers(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events)
            : base(lobby, events)
        {
            events.InitNewMap += Events_InitNewMap;
            events.PlayersPreparation += Events_PlayersPreparation;
            events.InRound += Events_InRound;
            events.RoundEndStats += Events_RoundEndStats;
            events.Countdown += Events_Countdown;
        }

        protected override void RemoveEvents(IBaseLobby lobby)
        {
            base.RemoveEvents(lobby);

            Events.InitNewMap -= Events_InitNewMap;
            Events.PlayersPreparation -= Events_PlayersPreparation;
            if (Events.InRound is { })
                Events.InRound -= Events_InRound;
            if (Events.RoundEndStats is { })
                Events.RoundEndStats -= Events_RoundEndStats;
            Events.Countdown -= Events_Countdown;
        }

        public override async Task<bool> RemovePlayer(ITDSPlayer player)
        {
            var lifes = player.Lifes;
            var worked = await base.RemovePlayer(player).ConfigureAwait(false);
            if (!worked)
                return false;

            if (lifes > 0)
                await Lobby.Deathmatch.RemovePlayerFromAlive(player).ConfigureAwait(false);
            else
            {
                SavePlayerRoundStats(player);
                player.SpectateHandler.SetSpectates(null);
            }

            return true;
        }

        protected virtual void Events_Countdown()
        {
        }

        private async ValueTask Events_InRound()
        {
            var teamPlayerAmountsJson = await Lobby.Teams.GetAmountInFightSyncDataJson().ConfigureAwait(false);

            await DoForAll(player =>
            {
                Lobby.Rounds.StartRoundForPlayer(player);
            }).ConfigureAwait(false);
            Lobby.Sync.TriggerEvent(ToClientEvent.AmountInFightSync, teamPlayerAmountsJson);
        }

        protected virtual void Events_PlayersPreparation()
        {
            DoForAll(player =>
            {
                Lobby.Rounds.SetPlayerReadyForRound(player, true);
                player.CurrentRoundStats?.Clear();
            });
        }

        private void Events_InitNewMap(MapDto mapDto)
        {
            SavePlayerLobbyStats = !mapDto.Info.IsNewMap && Lobby.IsOfficial;
        }

        private async ValueTask Events_RoundEndStats()
        {
            if (SavePlayerLobbyStats)
            {
                var playerRewards = await GetRewardsDatas().ConfigureAwait(false);

                await DoInMain(player =>
                {
                    if (playerRewards.TryGetValue(player, out var reward))
                        Lobby.Rounds.RewardPlayerForRound(player, reward);
                }).ConfigureAwait(false);
            }
        }

        private async Task<Dictionary<ITDSPlayer, RoundPlayerRewardsData>> GetRewardsDatas()
        {
            var playerRewards = new Dictionary<ITDSPlayer, RoundPlayerRewardsData>();
            var strBuilder = new StringBuilder();
            await DoForAll(player =>
            {
                SavePlayerRoundStats(player);
                if (ShouldRewardPlayerForRound)
                {
                    var model = new RoundPlayerRewardsData();
                    if (!AddPlayerRoundRewards(player, model))
                        return;
                    Lobby.Rounds.AddRoundRewardsMessage(player, strBuilder, model);
                    playerRewards.Add(player, model);
                }
                player.CurrentRoundStats?.Clear();
            }).ConfigureAwait(false);
            return playerRewards;
        }

        private void SavePlayerRoundStats(ITDSPlayer player)
        {
            if (player.LobbyStats is null)
                return;

            var to = player.LobbyStats;
            var from = player.CurrentRoundStats;
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

            if (from.Damage > 0)
            {
                if (from.Kills > 0)
                    player.Challenges.AddToChallenge(ChallengeType.Kills, from.Kills);
                if (from.Assists > 0)
                    player.Challenges.AddToChallenge(ChallengeType.Assists, from.Assists);
                player.Challenges.AddToChallenge(ChallengeType.Damage, from.Damage);
                player.Challenges.AddToChallenge(ChallengeType.RoundPlayed);
            }
        }

        private bool AddPlayerRoundRewards(ITDSPlayer player, RoundPlayerRewardsData toModel)
        {
            if (player.CurrentRoundStats is null)
                return false;
            if (player.Team is null || player.Team.IsSpectator)
                return false;
            if (!(Lobby.Entity.LobbyRewards is { } rewards))
                return false;

            if (rewards.MoneyPerKill != 0)
                toModel.KillsReward = (uint)(player.CurrentRoundStats.Kills * rewards.MoneyPerKill);
            if (rewards.MoneyPerAssist != 0)
                toModel.AssistsReward = (uint)(player.CurrentRoundStats.Assists * rewards.MoneyPerAssist);
            if (rewards.MoneyPerDamage != 0)
                toModel.DamageReward = (uint)(player.CurrentRoundStats.Damage * rewards.MoneyPerDamage);

            return true;
        }

        public void SetPlayerDataAlive(ITDSPlayer player)
        {
            if (player.Team is null)
                return;
            player.Lifes = (short)Lobby.Deathmatch.AmountLifes;
            player.Team.Players.AddAlive(player);
        }

        public void RespawnPlayer(ITDSPlayer player)
        {
            player.DeathSpawnTimer?.Kill();

            if (!(Lobby.Rounds.RoundStates.CurrentState is InRoundState))
            {
                player.DeathSpawnTimer = null;
                return;
            }

            player.DeathSpawnTimer = new TDSTimer(() =>
            {
                Lobby.Rounds.SetPlayerReadyForRound(player, false);
                Lobby.Rounds.StartRoundForPlayer(player);
                NAPI.Task.RunSafe(() => player.TriggerEvent(ToClientEvent.PlayerRespawned));
            }, (uint)Lobby.Entity.FightSettings.SpawnAgainAfterDeathMs);
        }
    }
}