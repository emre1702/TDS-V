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
using TDS_Server.Handler.Entities.TeamSystem;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Shared.Data.Utility;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.TeamHandlers
{
    public class BaseLobbyTeamsHandler
    {
        private List<ITeam> _teams = new List<ITeam>(3);
        private readonly SemaphoreSlim _teamsSemaphore = new SemaphoreSlim(1, 1);

        public BaseLobbyTeamsHandler(LobbyDb entity, BaseLobbyEventsHandler events)
        {
            InitTeams(entity);

            events.PlayerJoined += Events_PlayerJoined;
        }

        private async void InitTeams(LobbyDb entity)
        {
            await _teamsSemaphore.Do(() =>
            {
                _teams.Capacity = entity.Teams.Count;
                foreach (var teamEntity in entity.Teams.OrderBy(t => t.Index))
                {
                    var team = new Team(teamEntity);
                    _teams.Add(team);
                }
            });
        }

        public virtual void SetPlayerTeam(ITDSPlayer player, ITeam? team)
        {
            if (player.Team == team)
                return;

            NAPI.Task.Run(() =>
            {
                player.Team?.SyncRemovedPlayer(player);
                player.SetTeam(team, true);
                team?.SyncAddedPlayer(player);
            });
        }

        protected virtual async void Events_PlayerJoined(ITDSPlayer player, int teamIndex)
        {
            var team = await _teamsSemaphore.Do(() =>
            {
                if (teamIndex < 0)
                    teamIndex = SharedUtils.Rnd.Next(1, _teams.Count);
                return _teams[teamIndex];
            });

            SetPlayerTeam(player, team);
        }

        public Task Do(Action<List<ITeam>> action)
            => _teamsSemaphore.Do(() => action(_teams));

        public Task<T> Do<T>(Func<List<ITeam>, T> func)
            => _teamsSemaphore.Do(() => func(_teams));

        public List<ITeam> GetTeams() => _teams.ToList();

        public Task<ITeam> GetTeam(short teamIndex)
            => _teamsSemaphore.Do(() => _teams[teamIndex]);

        internal ITeam GetTeamWithFewestPlayer(List<ITeam> teams)
            => _teams.Skip(1).MinBy(t => t.Players.Count).Shuffle().First();
    }
}
