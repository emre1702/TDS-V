using GTANetworkAPI;
using System.Linq;
using System.Text;
using TDS_Common.Default;
using TDS_Common.Dto;
using System.Collections.Generic;
using TDS_Server.Dto;
using TDS_Server.Manager.Utility;
using TDS_Common.Manager.Utility;
using TDS_Server.Data.Models;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Arena
    {
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

            StringBuilder strbuilder = new StringBuilder();
            FuncIterateAllPlayers((character, team) =>
            {
                if (character.CurrentRoundStats is null)
                    return;
                if (team is null || team.IsSpectator)
                    return;

                uint killreward = 0;
                uint assistreward = 0;
                uint damagereward = 0;

                if (Entity.LobbyRewards.MoneyPerKill != 0)
                    killreward = (uint)(character.CurrentRoundStats.Kills * Entity.LobbyRewards.MoneyPerKill);
                if (Entity.LobbyRewards.MoneyPerAssist != 0)
                    assistreward = (uint)(character.CurrentRoundStats.Assists * Entity.LobbyRewards.MoneyPerAssist);
                if (Entity.LobbyRewards.MoneyPerDamage != 0)
                    damagereward = (uint)(character.CurrentRoundStats.Damage * Entity.LobbyRewards.MoneyPerDamage);

                character.GiveMoney(killreward + assistreward + damagereward);

                strbuilder.Append("#o#____________________#n#");
                strbuilder.AppendFormat(character.Language.ROUND_REWARD_INFO,
                        killreward == 0 ? "-" : killreward.ToString(),
                        assistreward == 0 ? "-" : assistreward.ToString(),
                        damagereward == 0 ? "-" : damagereward.ToString(),
                        killreward + assistreward + damagereward);
                strbuilder.Append("#n##o#____________________");

                character.SendMessage(strbuilder.ToString());
                strbuilder.Clear();
            });
        }

        private List<RoundPlayerRankingStat>? GetOrderedRoundRanking()
        {
            if (IsEmpty())
                return null;

            var list = Players
                .Where(p => p.CurrentRoundStats != null && p.Team is { } && !p.Team.IsSpectator)
                .Select(p => new RoundPlayerRankingStat(p))
                .ToList();

            float killsMult = SettingsManager.MultiplierRankingKills;
            float assistsMult = SettingsManager.MultiplierRankingAssists;
            float damageMult = SettingsManager.MultiplierRankingDamage;

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

        private void SetAllPlayersInCountdown()
        {
            FuncIterateAllPlayers((player, team) =>
            {
                if (team != null && !team.IsSpectator)
                {
                    RemoveAsSpectator(player);
                    team.SpectateablePlayers?.Add(player);
                }
                SetPlayerReadyForRound(player);
                NAPI.ClientEvent.TriggerClientEvent(player.Player, ToClientEvent.CountdownStart);
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
            SendAllPlayerEvent(ToClientEvent.AmountInFightSync, null, json);
        }

        protected void SaveAllPlayerRoundStats()
        {
            FuncIterateAllPlayers((player, team) =>
            {
                if (team is null || team.IsSpectator)
                    return;
                SavePlayerRoundStats(player);
            });
        }
    }
}
