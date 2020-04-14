using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Entities.TeamSystem;
using TDS_Server.Handler.Entities.Utility;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Lobby
    {
        public List<ITeam> Teams { get; set; }

        private static readonly Dictionary<TeamOrder, Func<ILanguage, string>> teamOrderDict = new Dictionary<TeamOrder, Func<ILanguage, string>>
        {
            [TeamOrder.Attack] = lang => lang.ORDER_ATTACK,
            [TeamOrder.StayBack] = lang => lang.ORDER_STAY_BACK,
            [TeamOrder.GoToBomb] = lang => lang.ORDER_GO_TO_BOMB,
            [TeamOrder.SpreadOut] = lang => lang.ORDER_SPREAD_OUT,
        };

        /*private int GetAmountTeamsWithPlayers()
        {
            return this.TeamPlayers.Values.Count(list => list.Count > 0);
        }*/

        protected ITeam GetTeamWithFewestPlayer()
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
            var oldPlayersList = Players.Values.ToList();
            ClearTeamPlayersLists();
            foreach (TDSPlayer player in oldPlayersList)
            {
                if (player.Team is null) // propably not (yet) in the lobby
                    continue;
                if (!player.Team.IsSpectator)
                {
                    player.Team = null;
                    player.Team = GetTeamWithFewestPlayer();
                }
                else
                    player.Team.Players.Add(player);
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

        public void SendTeamOrder(ITDSPlayer character, TeamOrder teamOrder)
        {
            if (!teamOrderDict.ContainsKey(teamOrder))
                return;
            if (character.Team is null)
                return;

            ITeam team = character.Team;
            Dictionary<ILanguage, string> texts = LangHelper.GetLangDictionary(teamOrderDict[teamOrder]);

            string str = $"[TEAM] {team.ChatColor}{character.DisplayName}: !$150|0|0$";
            team.FuncIterate((target, _) =>
            {
                target.SendMessage(str + texts[target.Language]);
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
