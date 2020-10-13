using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;

namespace TDS_Server.LobbySystem.TeamHandlers
{
    public class FightLobbyTeamsHandler : BaseLobbyTeamsHandler, IFightLobbyTeamsHandler
    {
        private static readonly Dictionary<TeamOrder, Func<ILanguage, string>> _teamOrdersDict = new Dictionary<TeamOrder, Func<ILanguage, string>>
        {
            [TeamOrder.Attack] = lang => lang.ORDER_ATTACK,
            [TeamOrder.StayBack] = lang => lang.ORDER_STAY_BACK,
            [TeamOrder.GoToBomb] = lang => lang.ORDER_GO_TO_BOMB,
            [TeamOrder.SpreadOut] = lang => lang.ORDER_SPREAD_OUT,
        };

        private readonly LangHelper _langHelper;

        public FightLobbyTeamsHandler(IFightLobby lobby, IFightLobbyEventsHandler events, LangHelper langHelper, ITeamsProvider teamsProvider)
            : base(lobby, events, teamsProvider)
        {
            _langHelper = langHelper;
        }

        public void SendTeamOrder(ITDSPlayer player, TeamOrder teamOrder)
        {
            if (!_teamOrdersDict.ContainsKey(teamOrder))
                return;
            if (player.Team is null)
                return;

            var team = player.Team;
            Dictionary<ILanguage, string> texts = _langHelper.GetLangDictionary(_teamOrdersDict[teamOrder]);

            string str = $"[TEAM] {team.Chat.Color}{player.DisplayName}: !$150|0|0$";
            team.Chat.Send(lang => str + texts[lang]);
        }

        protected Task ClearTeamPlayersAmounts()
        {
            return Do(teams =>
            {
                foreach (var team in teams)
                    team.Players.ClearRound();
            });
        }

        protected Task MixTeams()
        {
            return Do(teams =>
            {
                foreach (var team in teams)
                    team.Sync.SyncChanges = false;

                var oldPlayersList = Lobby.Players.GetPlayers();

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
                    team.Sync.SyncChanges = true;
                    team.Sync.SyncAllPlayers();
                }
            });
        }

        private void ClearTeamPlayersLists(ITeam[] teams)
        {
            foreach (var team in teams)
                team.Players.ClearLists();
        }

        public Task<ITeam?> GetNextTeamWithSpectatablePlayers(ITeam? start)
            => GetNextTeamWithSpectatablePlayers(start?.Entity.Index ?? 0);

        public Task<ITeam?> GetNextTeamWithSpectatablePlayers(short startIndex)
        {
            return Do(teams =>
            {
                if (teams.Length <= 1)
                    return null;

                var index = startIndex;
                do
                {
                    if (++index > teams.Length - 1)
                        index = 0;
                } while (teams[index].Players.HasAnySpectatable && index != startIndex);

                var team = teams[index];
                if (team.Players.HasAnySpectatable)
                    return null;

                return team;
            });
        }

        public Task<ITeam> GetNextNonSpectatorTeam(ITeam start)
        {
            return GetNextNonSpectatorTeam(start.Entity.Index);
        }

        public Task<ITeam> GetNextNonSpectatorTeam(short startIndex)
        {
            var startIndexToIterate = startIndex;
            return Do(teams =>
            {
                do
                {
                    if (++startIndexToIterate == teams.Length - 1)
                        startIndexToIterate = 0;
                } while (teams[startIndex].IsSpectator && startIndexToIterate != startIndex);

                return teams[startIndexToIterate];
            });
        }

        public Task<ITeam?> GetPreviousTeamWithSpectatablePlayers(ITeam? start)
           => GetPreviousNonSpectatorTeamWithPlayers(start?.Entity.Index ?? 0);

        public Task<ITeam?> GetPreviousNonSpectatorTeamWithPlayers(short startIndex)
        {
            return Do(teams =>
            {
                if (teams.Length <= 1)
                    return null;

                var index = (int)startIndex;
                do
                {
                    if (--index < 0)
                        index = teams.Length - 1;
                } while (teams[index].Players.HasAnySpectatable && index != startIndex);

                var team = teams[index];
                if (team.Players.HasAnySpectatable)
                    return null;

                return team;
            });
        }

        public async Task<string> GetAmountInFightSyncDataJson()
        {
            var teamPlayerAmounts = await Lobby.Teams.Do(teams => teams.Skip(1).Select(t => t.SyncedData).Select(t => t.AmountPlayers))
                .ConfigureAwait(false);
            return Serializer.ToClient(teamPlayerAmounts);
        }
    }
}
