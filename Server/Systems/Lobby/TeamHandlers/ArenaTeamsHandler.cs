﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;
using MoreLinq;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Helper;

namespace TDS.Server.LobbySystem.TeamHandlers
{
    public class ArenaTeamsHandler : RoundFightLobbyTeamsHandler, IArenaTeamsHandler
    {
        public ArenaTeamsHandler(IArena lobby, IRoundFightLobbyEventsHandler events, LangHelper langHelper, ITeamsProvider teamsProvider)
            : base(lobby, events, langHelper, teamsProvider)
        {
            events.TeamPreparation += Events_TeamPreparation;
        }

        protected override void RemoveEvents(IBaseLobby lobby)
        {
            base.RemoveEvents(lobby);

            Events.TeamPreparation -= Events_TeamPreparation;
        }

        public async Task BalanceCurrentTeams()
        {
            var playersToPutIntoOtherTeam = new List<ITDSPlayer>();

            await DoForList(teams =>
            {
                var teamWithFewestPlayers = teams.Skip(1).MinBy(t => t.Players.Amount).Shuffle().FirstOrDefault();
                var teamWithMostPlayers = teams.Skip(1).MaxBy(t => t.Players.Amount).Shuffle().FirstOrDefault();

                while (teamWithFewestPlayers is { } && teamWithMostPlayers is { }
                    && teamWithMostPlayers.Players.Amount - teamWithFewestPlayers.Players.Amount > 1)
                {
                    var playerToPutIntoOtherTeam = teamWithMostPlayers.Players.Last();
                    SetPlayerTeam(playerToPutIntoOtherTeam!, teamWithFewestPlayers);

                    teamWithFewestPlayers = teams.Skip(1).MinBy(t => t.Players.Amount).Shuffle().FirstOrDefault();
                    teamWithMostPlayers = teams.Skip(1).MaxBy(t => t.Players.Amount).Shuffle().FirstOrDefault();
                }
            }).ConfigureAwait(false);

            NAPI.Task.RunSafe(() =>
            {
                foreach (var player in playersToPutIntoOtherTeam)
                    Lobby.Notifications.Send(lang => string.Format(lang.BALANCE_TEAM_INFO, player.DisplayName));
            });
        }

        private void Events_TeamPreparation()
        {
            if (Lobby.Entity.LobbyRoundSettings.MixTeamsAfterRound)
                MixTeams();
        }
    }
}
