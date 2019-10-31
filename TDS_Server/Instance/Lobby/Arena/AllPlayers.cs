using GTANetworkAPI;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using TDS_Common.Default;
using TDS_Common.Dto;
using System.Collections.Generic;
using TDS_Server.Instance.Player;
using TDS_Server.Dto;
using TDS_Server.Manager.Utility;
using System;

namespace TDS_Server.Instance.Lobby
{
    partial class Arena
    {
        private void RewardAllPlayer()
        {
            if (!LobbyEntity.IsOfficial)
                return;
            if (IsEmpty())
                return;
            if (LobbyEntity.LobbyRewards is null)
                return;
            if (LobbyEntity.LobbyRewards.MoneyPerKill == 0 && LobbyEntity.LobbyRewards.MoneyPerAssist == 0 && LobbyEntity.LobbyRewards.MoneyPerDamage == 0)
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

                if (LobbyEntity.LobbyRewards.MoneyPerKill != 0)
                    killreward = (uint)(character.CurrentRoundStats.Kills * LobbyEntity.LobbyRewards.MoneyPerKill);
                if (LobbyEntity.LobbyRewards.MoneyPerAssist != 0)
                    assistreward = (uint)(character.CurrentRoundStats.Assists * LobbyEntity.LobbyRewards.MoneyPerAssist);
                if (LobbyEntity.LobbyRewards.MoneyPerDamage != 0)
                    damagereward = (uint)(character.CurrentRoundStats.Damage * LobbyEntity.LobbyRewards.MoneyPerDamage);

                character.GiveMoney(killreward + assistreward + damagereward);

                strbuilder.Append("#o#____________________#n#");
                strbuilder.AppendFormat(character.Language.ROUND_REWARD_INFO,
                        killreward == 0 ? "-" : killreward.ToString(),
                        assistreward == 0 ? "-" : assistreward.ToString(),
                        damagereward == 0 ? "-" : damagereward.ToString(),
                        killreward + assistreward + damagereward);
                strbuilder.Append("#n##o#____________________");

                NAPI.Chat.SendChatMessageToPlayer(character.Client, strbuilder.ToString());
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
            FuncIterateAllPlayers((character, team) =>
            {
                if (team != null && !team.IsSpectator)
                {
                    RemoveAsSpectator(character);
                    team.SpectateablePlayers?.Add(character);
                }
                SetPlayerReadyForRound(character);
                NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.CountdownStart);
            });
        }

        private void StartRoundForAllPlayer()
        {
            FuncIterateAllPlayers((player, team) =>
            {
                StartRoundForPlayer(player);
            });

            SyncedTeamPlayerAmountDto[] amounts = Teams.Skip(1).Select(t => t.SyncedTeamData).Select(t => t.AmountPlayers).ToArray();
            string json = JsonConvert.SerializeObject(amounts);
            SendAllPlayerEvent(DToClientEvent.AmountInFightSync, null, json);
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