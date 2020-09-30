using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GTANetworkAPI;
using MoreLinq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.TeamsHandlers;
using TDS_Server.Handler.Entities.TeamSystem;
using TDS_Shared.Data.Utility;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.TeamHandlers
{
    public class BaseLobbyTeamsHandler : IBaseLobbyTeamsHandler
    {
        protected readonly IBaseLobby Lobby;
        private readonly List<ITeam> _teams = new List<ITeam>(3);
        private readonly SemaphoreSlim _teamsSemaphore = new SemaphoreSlim(1, 1);

        public BaseLobbyTeamsHandler(IBaseLobby lobby, IBaseLobbyEventsHandler events)
        {
            Lobby = lobby;
            InitTeams(Lobby.Entity);

            events.PlayerJoined += Events_PlayerJoined;
        }

        private void InitTeams(LobbyDb entity)
        {
            _teams.Capacity = entity.Teams.Count;
            foreach (var teamEntity in entity.Teams.OrderBy(t => t.Index))
            {
                var team = new Team(teamEntity);
                _teams.Add(team);
            }
        }

        public virtual Task SetPlayerTeam(ITDSPlayer player, ITeam? team)
        {
            if (player.Team == team)
                return Task.CompletedTask;

            return NAPI.Task.RunWait(() =>
            {
                player.Team?.SyncRemovedPlayer(player);
                player.SetTeam(team, true);
                team?.SyncAddedPlayer(player);
            });
        }

        protected virtual async ValueTask Events_PlayerJoined((ITDSPlayer Player, int TeamIndex) data)
        {
            var team = await _teamsSemaphore.Do(() =>
            {
                if (data.TeamIndex < 0)
                    data.TeamIndex = SharedUtils.Rnd.Next(1, _teams.Count);
                return _teams[data.TeamIndex];
            });

            await SetPlayerTeam(data.Player, team);
        }

        public Task Do(Action<List<ITeam>> action)
            => _teamsSemaphore.Do(() => action(_teams));

        public Task<T> Do<T>(Func<List<ITeam>, T> func)
            => _teamsSemaphore.Do(() => func(_teams));

        public List<ITeam> GetTeams() => _teams.ToList();

        public Task<ITeam> GetTeam(short teamIndex)
            => _teamsSemaphore.Do(() => _teams[teamIndex]);

        public Task<ITeam> GetTeamWithFewestPlayer()
            => _teamsSemaphore.Do(() => GetTeamWithFewestPlayer(_teams));

        public ITeam GetTeamWithFewestPlayer(List<ITeam> teams)
            => teams.Skip(1).MinBy(t => t.Players.Count).Shuffle().First();
    }
}
