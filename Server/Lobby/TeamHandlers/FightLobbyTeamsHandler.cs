using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;
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

        public FightLobbyTeamsHandler(IFightLobby lobby, IBaseLobbyEventsHandler events, LangHelper langHelper)
            : base(lobby, events)
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
                    team.SyncAllPlayers();
                }
            });
        }

        private void ClearTeamPlayersLists(ITeam[] teams)
        {
            foreach (var entry in teams)
            {
                entry.Players.Clear();
                entry.AlivePlayers?.Clear();
                entry.SpectateablePlayers?.Clear();
            }
        }

        public Task<ITeam?> GetNextNonSpectatorTeamWithPlayers(ITeam? start)
            => GetNextNonSpectatorTeamWithPlayers(start?.Entity.Index ?? 0);

        public Task<ITeam?> GetNextNonSpectatorTeamWithPlayers(short startIndex)
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
                } while (teams[index].SpectateablePlayers?.Any() != true && index != startIndex);

                var team = teams[index];
                if (team.SpectateablePlayers?.Any() != true)
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

        public Task<ITeam?> GetPreviousNonSpectatorTeamWithPlayers(ITeam? start)
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
                } while (teams[index].SpectateablePlayers?.Any() != true && index != startIndex);

                var team = teams[index];
                if (team.SpectateablePlayers?.Any() != true)
                    return null;

                return team;
            });
        }

        public async Task<string> GetAmountInFightSyncDataJson()
        {
            var teamPlayerAmounts = await Lobby.Teams.Do(teams => teams.Skip(1).Select(t => t.SyncedTeamData).Select(t => t.AmountPlayers))
                .ConfigureAwait(false);
            return Serializer.ToClient(teamPlayerAmounts);
        }
    }
}
