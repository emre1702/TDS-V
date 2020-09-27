using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Models;
using TDS_Server.Data.Models.Map;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.MapVotings;
using TDS_Server.LobbySystem.RoundsHandlers.Datas.RoundStates;
using TDS_Server.LobbySystem.TeamHandlers;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Default;

namespace TDS_Server.LobbySystem.Players
{
    public class RoundFightLobbyPlayers : FightLobbyPlayers
    {
        protected new IRoundFightLobby Lobby => (IRoundFightLobby)base.Lobby;

        protected bool SavePlayerLobbyStats { get; private set; }

        protected bool ShouldRewardPlayerForRound => Lobby.Entity.LobbyRewards is { } rewards &&
            (rewards.MoneyPerAssist != 0 || rewards.MoneyPerDamage != 0 || rewards.MoneyPerKill != 0);

        public RoundFightLobbyPlayers(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events)
            : base(lobby, events)
        {
            events.InitNewMap += Events_InitNewMap;
            events.PlayersPreparation += Events_PlayersPreparation;
            events.InRound += Events_InRound;
            events.RoundEnd += Events_RoundEnd;
            events.RoundEndStats += Events_RoundEndStats;
            events.Countdown += Events_Countdown;
        }

        public override async Task<bool> AddPlayer(ITDSPlayer player, int teamIndex)
        {
            var worked = await base.AddPlayer(player, teamIndex);
            if (!worked)
                return false;
            await SendPlayerRoundInfoOnJoin(player);
            return true;
        }

        protected virtual void Events_Countdown()
        {
        }

        private void Events_InRound()
        {
            //Todo: Implement this
            //StartRoundForAllPlayer();
        }

        protected virtual void Events_PlayersPreparation()
        {
        }

        private void Events_InitNewMap(MapDto mapDto)
        {
            SavePlayerLobbyStats = !mapDto.Info.IsNewMap && Lobby.IsOfficial;
        }

        protected virtual async ValueTask Events_RoundEnd()
        {
            var mapId = Lobby.CurrentMap?.BrowserSyncedData.Id ?? 0;
            await DoInMain(player =>
            {
                var noTeamOrSpectator = player.Team is null || player.Team.IsSpectator;
                var roundEndReasonText = Lobby.CurrentRoundEndReason.MessageProvider(player.Language);

                player.TriggerEvent(ToClientEvent.RoundEnd, noTeamOrSpectator, roundEndReasonText, mapId);
                player.Lifes = 0;
            });
        }

        private async ValueTask Events_RoundEndStats()
        {
            if (SavePlayerLobbyStats)
            {
                var playerRewards = await GetRewardsDatas();

                await DoInMain(player =>
                {
                    if (playerRewards.TryGetValue(player, out var reward))
                        RewardPlayerForRound(player, reward);
                });
            }
        }

        private async Task<Dictionary<ITDSPlayer, RoundPlayerRewardsData>> GetRewardsDatas()
        {
            var playerRewards = new Dictionary<ITDSPlayer, RoundPlayerRewardsData>();
            var strBuilder = new StringBuilder();
            await Do(player =>
            {
                SavePlayerRoundStats(player);
                if (ShouldRewardPlayerForRound)
                {
                    var model = new RoundPlayerRewardsData();
                    if (!AddPlayerRoundRewards(player, model))
                        return;
                    AddRoundRewardsMessage(player, strBuilder, model);
                    playerRewards.Add(player, model);
                }
            });
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
                    player.AddToChallenge(ChallengeType.Kills, from.Kills);
                if (from.Assists > 0)
                    player.AddToChallenge(ChallengeType.Assists, from.Assists);
                player.AddToChallenge(ChallengeType.Damage, from.Damage);
                player.AddToChallenge(ChallengeType.RoundPlayed);
            }

            from.Clear();
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

        private void AddRoundRewardsMessage(ITDSPlayer player, StringBuilder useStringBuilder, RoundPlayerRewardsData toModel)
        {
            useStringBuilder.Append("#o#____________________#n#");
            useStringBuilder.AppendFormat(player.Language.ROUND_REWARD_INFO,
                    toModel.KillsReward == 0 ? "-" : toModel.KillsReward.ToString(),
                    toModel.AssistsReward == 0 ? "-" : toModel.AssistsReward.ToString(),
                    toModel.DamageReward == 0 ? "-" : toModel.DamageReward.ToString(),
                    toModel.KillsReward + toModel.AssistsReward + toModel.DamageReward);
            useStringBuilder.Append("#n##o#____________________");

            toModel.Message = useStringBuilder.ToString();
            useStringBuilder.Clear();
        }

        private void RewardPlayerForRound(ITDSPlayer player, RoundPlayerRewardsData rewardsData)
        {
            player.GiveMoney(rewardsData.KillsReward + rewardsData.AssistsReward + rewardsData.DamageReward);
            player.SendChatMessage(rewardsData.Message);
        }

        protected virtual async Task SendPlayerRoundInfoOnJoin(ITDSPlayer player)
        {
            var teamPlayerAmounts = await Lobby.Teams.Do(teams => teams.Skip(1).Select(t => t.SyncedTeamData).Select(t => t.AmountPlayers));
            var teamPlayerAmountsJson = Serializer.ToClient(teamPlayerAmounts);

            NAPI.Task.Run(() =>
            {
                if (Lobby.CurrentMap is { } map)
                    player.TriggerEvent(ToClientEvent.MapChange, map.ClientSyncedDataJson);
                player.TriggerEvent(ToClientEvent.AmountInFightSync, teamPlayerAmountsJson);

                if (Lobby.Rounds.RoundStates.CurrentState is CountdownState)
                    player.TriggerEvent(ToClientEvent.CountdownStart, true, Lobby.Rounds.RoundStates.TimeToNextStateMs);
                else if (Lobby.Rounds.RoundStates.CurrentState is InRoundState)
                    player.TriggerEvent(ToClientEvent.RoundStart, true, Lobby.Rounds.RoundStates.TimeInStateMs);
            });
        }
    }
}
