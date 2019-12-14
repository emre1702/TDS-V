using GTANetworkAPI;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Server.Interfaces;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.LobbyInstances
{
    partial class Lobby
    {
        public List<Team> Teams { get; set; }

        private static readonly Dictionary<ETeamOrder, Func<ILanguage, string>> teamOrderDict = new Dictionary<ETeamOrder, Func<ILanguage, string>>
        {
            [ETeamOrder.Attack] = lang => lang.ORDER_ATTACK,
            [ETeamOrder.StayBack] = lang => lang.ORDER_STAY_BACK,
            [ETeamOrder.GoToBomb] = lang => lang.ORDER_GO_TO_BOMB,
            [ETeamOrder.SpreadOut] = lang => lang.ORDER_SPREAD_OUT,
        };

        /*private int GetAmountTeamsWithPlayers()
        {
            return this.TeamPlayers.Values.Count(list => list.Count > 0);
        }*/

        protected Team GetTeamWithFewestPlayer()
        {
            return Teams.Skip(1).MinBy(t => t.Players.Count).Shuffle().FirstOrDefault();
        }

        private void ClearTeamPlayersLists()
        {
            foreach (var entry in Teams)
            {
                entry.Players.Clear();
                entry.AlivePlayers?.Clear();
                entry.SpectateablePlayers?.Clear();
            }
        }

        protected void MixTeams()
        {
            var oldPlayersList = Players.ToList();
            ClearTeamPlayersLists();
            foreach (TDSPlayer character in oldPlayersList)
            {
                if (character.Team is null) // propably not (yet) in the lobby
                    continue;
                if (!character.Team.IsSpectator)
                {
                    character.Team = null;
                    character.Team = GetTeamWithFewestPlayer();
                }
                else
                    character.Team.Players.Add(character);
            }

            foreach (var team in Teams)
            {
                if (!team.IsSpectator)
                    team.SyncAllPlayers();
            }
        }

        protected void ClearTeamPlayersAmounts()
        {
            foreach (var team in Teams)
            {
                team.SyncedTeamData.AmountPlayers.AmountAlive = 0;
                team.SyncedTeamData.AmountPlayers.Amount = 0;
            }
        }

        public static void SendTeamOrder(TDSPlayer character, ETeamOrder teamOrder)
        {
            if (!teamOrderDict.ContainsKey(teamOrder))
                return;
            if (character.Team is null)
                return;

            Team team = character.Team;
            Dictionary<ILanguage, string> texts = LangUtils.GetLangDictionary(teamOrderDict[teamOrder]);

            string str = $"[TEAM] {team.ChatColor}{character.DisplayName}: !{{150|0|0}}";
            team.FuncIterate((target, _) =>
            {
                NAPI.Chat.SendChatMessageToPlayer(target.Client, str + texts[target.Language]);
            });
        }

        public void BalanceCurrentTeams()
        {
            var teamWithFewestPlayers = Teams.Skip(1).MinBy(t => t.Players.Count).Shuffle().FirstOrDefault();
            var teamWithMostPlayers = Teams.Skip(1).MaxBy(t => t.Players.Count).Shuffle().FirstOrDefault();

            while (teamWithFewestPlayers is { } && teamWithMostPlayers is { }
                && teamWithMostPlayers.Players.Count - teamWithFewestPlayers.Players.Count > 1)
            {
                var playerToPutIntoOtherTeam = teamWithMostPlayers.Players.Last();
                SetPlayerTeam(playerToPutIntoOtherTeam, teamWithFewestPlayers);
                SendAllPlayerLangNotification(lang => string.Format(lang.BALANCE_TEAM_INFO, playerToPutIntoOtherTeam.DisplayName));

                teamWithFewestPlayers = Teams.Skip(1).MinBy(t => t.Players.Count).Shuffle().FirstOrDefault();
                teamWithMostPlayers = Teams.Skip(1).MaxBy(t => t.Players.Count).Shuffle().FirstOrDefault();
            }

           
        }
    }
}