using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Models;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Arena
    {
        #region Protected Methods

        protected void SaveAllPlayerRoundStats()
        {
            FuncIterateAllPlayers((player, team) =>
            {
                if (team is null || team.IsSpectator)
                    return;
                SavePlayerRoundStats(player);
            });
        }

        #endregion Protected Methods

        #region Private Methods

        private List<RoundPlayerRankingStat>? GetOrderedRoundRanking()
        {
            if (IsEmpty())
                return null;

            var list = Players.Values
                .Where(p => p.CurrentRoundStats is { } && p.Team is { } && !p.Team.IsSpectator)
                .Select(p => new RoundPlayerRankingStat(p))
                .ToList();

            float killsMult = SettingsHandler.ServerSettings.MultiplierRankingKills;
            float assistsMult = SettingsHandler.ServerSettings.MultiplierRankingAssists;
            float damageMult = SettingsHandler.ServerSettings.MultiplierRankingDamage;

            foreach (var ranking in list)
            {
                ranking.Points = (int)(ranking.Kills * killsMult + ranking.Assists * assistsMult + ranking.Damage * damageMult);
            }

            list.Sort((a, b) => a.Points.CompareTo(b.Points) * -1);

            int place = 0;
            foreach (var ranking in list)
            {
                ranking.Place = ++place;
            }

            return list;
        }

        private void RewardAllPlayer()
        {
            if (!Entity.IsOfficial)
                return;
            if (IsEmpty())
                return;
            if (Entity.LobbyRewards is null)
                return;
            if (Entity.LobbyRewards.MoneyPerKill == 0 && Entity.LobbyRewards.MoneyPerAssist == 0 && Entity.LobbyRewards.MoneyPerDamage == 0)
                return;

            var strbuilder = new StringBuilder();
            FuncIterateAllPlayers((player, team) =>
            {
                if (player.CurrentRoundStats is null)
                    return;
                if (team is null || team.IsSpectator)
                    return;

                uint killreward = 0;
                uint assistreward = 0;
                uint damagereward = 0;

                if (Entity.LobbyRewards.MoneyPerKill != 0)
                    killreward = (uint)(player.CurrentRoundStats.Kills * Entity.LobbyRewards.MoneyPerKill);
                if (Entity.LobbyRewards.MoneyPerAssist != 0)
                    assistreward = (uint)(player.CurrentRoundStats.Assists * Entity.LobbyRewards.MoneyPerAssist);
                if (Entity.LobbyRewards.MoneyPerDamage != 0)
                    damagereward = (uint)(player.CurrentRoundStats.Damage * Entity.LobbyRewards.MoneyPerDamage);

                player.GiveMoney(killreward + assistreward + damagereward);

                strbuilder.Append("#o#____________________#n#");
                strbuilder.AppendFormat(player.Language.ROUND_REWARD_INFO,
                        killreward == 0 ? "-" : killreward.ToString(),
                        assistreward == 0 ? "-" : assistreward.ToString(),
                        damagereward == 0 ? "-" : damagereward.ToString(),
                        killreward + assistreward + damagereward);
                strbuilder.Append("#n##o#____________________");

                player.SendChatMessage(strbuilder.ToString());
                strbuilder.Clear();
            });
        }

        private void SetAllPlayersInCountdown()
        {
            var mapCenter = _currentMap?.LimitInfo.Center.SwitchNamespace();
            FuncIterateAllPlayers((player, team) =>
            {
                if (team?.IsSpectator == false)
                {
                    RemoveAsSpectator(player);
                    team.SpectateablePlayers?.Add(player);
                }
                else
                {
                    MakeSurePlayerSpectatesAnyone(player);
                    if (player.Spectates is { })
                        player.Position = (mapCenter?.ToVector3() ?? player.Spectates.Position).AddToZ(10);
                }
                SetPlayerReadyForRound(player);
                player.CurrentRoundStats?.Clear();
                player.TriggerEvent(ToClientEvent.CountdownStart, team is null || team.IsSpectator);
            });
        }

        private void StartRoundForAllPlayer()
        {
            FuncIterateAllPlayers((player, team) =>
            {
                StartRoundForPlayer(player);
            });

            SyncedTeamPlayerAmountDto[] amounts = Teams.Skip(1).Select(t => t.SyncedTeamData).Select(t => t.AmountPlayers).ToArray();
            string json = Serializer.ToClient(amounts);
            TriggerEvent(ToClientEvent.AmountInFightSync, json);
        }

        #endregion Private Methods
    }
}
