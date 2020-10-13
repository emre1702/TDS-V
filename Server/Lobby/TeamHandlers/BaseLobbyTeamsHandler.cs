using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GTANetworkAPI;
using MoreLinq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Shared.Data.Utility;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.TeamHandlers
{
    public class BaseLobbyTeamsHandler : IBaseLobbyTeamsHandler
    {
        public int Count => _teams.Length;

        protected readonly IBaseLobby Lobby;
        protected readonly IBaseLobbyEventsHandler Events;
        private readonly ITeamsProvider _teamsProvider;

        private ITeam[] _teams = Array.Empty<ITeam>();
        private readonly SemaphoreSlim _teamsSemaphore = new SemaphoreSlim(1, 1);

        public BaseLobbyTeamsHandler(IBaseLobby lobby, IBaseLobbyEventsHandler events, ITeamsProvider teamsProvider)
        {
            Lobby = lobby;
            Events = events;
            _teamsProvider = teamsProvider;
            InitTeams(Lobby.Entity);

            events.PlayerJoined += Events_PlayerJoined;
            events.RemoveAfter += RemoveEvents;
        }

        protected virtual void RemoveEvents(IBaseLobby lobby)
        {
            if (Events.PlayerJoined is { })
                Events.PlayerJoined -= Events_PlayerJoined;
            Events.RemoveAfter -= RemoveEvents;
        }

        private void InitTeams(LobbyDb entity)
        {
            _teams = entity.Teams.Any() ? new ITeam[entity.Teams.Max(t => t.Index + 1)] : Array.Empty<ITeam>();
            foreach (var teamEntity in entity.Teams.OrderBy(t => t.Index))
            {
                var team = _teamsProvider.Create(teamEntity);
                _teams[team.Entity.Index] = team;
            }
        }

        public virtual void SetPlayerTeam(ITDSPlayer player, ITeam? team)
        {
            if (player.Team == team)
                return;

            player.SetTeam(team, true);
        }

        protected virtual async ValueTask Events_PlayerJoined((ITDSPlayer Player, int TeamIndex) data)
        {
            var team = await _teamsSemaphore.Do(() =>
            {
                if (data.TeamIndex < 0)
                    data.TeamIndex = SharedUtils.Rnd.Next(1, _teams.Length);
                return _teams[data.TeamIndex];
            }).ConfigureAwait(false);

            SetPlayerTeam(data.Player, team);
        }

        public Task Do(Action<ITeam[]> action)
            => _teamsSemaphore.Do(() => action(_teams));

        public Task<T> Do<T>(Func<ITeam[], T> func)
            => _teamsSemaphore.Do(() => func(_teams));

        public List<ITeam> GetTeams() => _teams.ToList();

        public Task<ITeam> GetTeam(short teamIndex)
            => _teamsSemaphore.Do(() => _teams[teamIndex]);

        public Task<ITeam> GetTeamWithFewestPlayer()
            => _teamsSemaphore.Do(() => GetTeamWithFewestPlayer(_teams));

        public ITeam GetTeamWithFewestPlayer(ITeam[] teams)
            => teams.Skip(1).MinBy(t => t.Players.Amount).Shuffle().First();
    }
}
