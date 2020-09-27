using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Players;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Shared.Data.Enums;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.TeamHandlers
{
    public class FightLobbyTeamsHandler : BaseLobbyTeamsHandler
    {
        private static readonly Dictionary<TeamOrder, Func<ILanguage, string>> _teamOrdersDict = new Dictionary<TeamOrder, Func<ILanguage, string>>
        {
            [TeamOrder.Attack] = lang => lang.ORDER_ATTACK,
            [TeamOrder.StayBack] = lang => lang.ORDER_STAY_BACK,
            [TeamOrder.GoToBomb] = lang => lang.ORDER_GO_TO_BOMB,
            [TeamOrder.SpreadOut] = lang => lang.ORDER_SPREAD_OUT,
        };

        private readonly LangHelper _langHelper;
        private readonly IBaseLobbyPlayers _players;

        public FightLobbyTeamsHandler(LobbyDb entity, IBaseLobbyEventsHandler events, LangHelper langHelper, IBaseLobbyPlayers players)
            : base(entity, events)
        {
            _langHelper = langHelper;
            _players = players;
        }

        public void SendTeamOrder(ITDSPlayer player, TeamOrder teamOrder)
        {
            if (!_teamOrdersDict.ContainsKey(teamOrder))
                return;
            if (player.Team is null)
                return;

            var team = player.Team;
            Dictionary<ILanguage, string> texts = _langHelper.GetLangDictionary(_teamOrdersDict[teamOrder]);

            string str = $"[TEAM] {team.ChatColor}{player.DisplayName}: !$150|0|0$";
            team.FuncIterate(target =>
            {
                target.SendChatMessage(str + texts[target.Language]);
            });
        }

        protected Task ClearTeamPlayersAmounts()
        {
            return Do(teams =>
            {
                foreach (var team in teams)
                {
                    team.AlivePlayers?.Clear();
                    team.SyncedTeamData.AmountPlayers.AmountAlive = 0;
                    team.SyncedTeamData.AmountPlayers.Amount = 0;
                }
            });
        }

        protected Task MixTeams()
        {
            return Do(teams =>
            {
                var oldPlayersList = _players.GetPlayers();
                ClearTeamPlayersLists(teams);
                foreach (var player in oldPlayersList)
                {
                    player.ResetVoiceToAndFrom();
                    if (player.Team is null) // propably not (yet) in the lobby
                        continue;
                    if (player.Team.IsSpectator)
                        player.Team.Players.Add(player);
                    else
                        player.SetTeam(GetTeamWithFewestPlayer(teams), true);
                }

                foreach (var team in teams)
                {
                    team.SyncAllPlayers();
                }
            });
        }

        private void ClearTeamPlayersLists(List<ITeam> teams)
        {
            foreach (var entry in teams)
            {
                entry.Players.Clear();
                entry.AlivePlayers?.Clear();
                entry.SpectateablePlayers?.Clear();
            }
        }

        internal Task<ITeam?> GetNextNonSpectatorTeamWithPlayers(ITeam? start)
            => GetNextNonSpectatorTeamWithPlayers(start?.Entity.Index ?? 0);

        internal Task<ITeam?> GetNextNonSpectatorTeamWithPlayers(short startIndex)
        {
            return Do(teams =>
            {
                if (teams.Count <= 1)
                    return null;

                var index = startIndex;
                do
                {
                    if (++index > teams.Count - 1)
                        index = 0;
                } while (teams[index].SpectateablePlayers?.Any() != true && index != startIndex);

                var team = teams[index];
                if (team.SpectateablePlayers?.Any() != true)
                    return null;

                return team;
            });
        }

        internal Task<ITeam> GetNextNonSpectatorTeam(ITeam start)
        {
            return GetNextNonSpectatorTeam(start.Entity.Index);
        }

        private Task<ITeam> GetNextNonSpectatorTeam(short startIndex)
        {
            var startIndexToIterate = startIndex;
            return Do(teams =>
            {
                do
                {
                    if (++startIndexToIterate == teams.Count - 1)
                        startIndexToIterate = 0;
                } while (teams[startIndex].IsSpectator && startIndexToIterate != startIndex);

                return teams[startIndexToIterate];
            });
        }

        internal Task<ITeam?> GetPreviousNonSpectatorTeamWithPlayers(ITeam? start)
           => GetPreviousNonSpectatorTeamWithPlayers(start?.Entity.Index ?? 0);

        internal Task<ITeam?> GetPreviousNonSpectatorTeamWithPlayers(short startIndex)
        {
            return Do(teams =>
            {
                if (teams.Count <= 1)
                    return null;

                var index = (int)startIndex;
                do
                {
                    if (--index < 0)
                        index = teams.Count - 1;
                } while (teams[index].SpectateablePlayers?.Any() != true && index != startIndex);

                var team = teams[index];
                if (team.SpectateablePlayers?.Any() != true)
                    return null;

                return team;
            });
        }
    }
}
