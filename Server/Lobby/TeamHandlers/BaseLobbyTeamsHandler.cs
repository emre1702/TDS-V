using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Entities.TeamSystem;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.TeamHandlers
{
    public class BaseLobbyTeamsHandler
    {
        public List<ITeam> Teams { get; set; } = new List<ITeam>(3);

        public BaseLobbyTeamsHandler(LobbyDb entity)
        {
            InitTeams(entity);
        }

        public virtual void SetPlayerTeam(ITDSPlayer player, ITeam? team)
        {
            if (player.Team == team)
                return;

            player.Team?.SyncRemovedPlayer(player);
            player.SetTeam(team, true);
            team?.SyncAddedPlayer(player);
        }

        private void InitTeams(LobbyDb entity)
        {
            Teams.Capacity = entity.Teams.Count;
            foreach (var teamEntity in entity.Teams.OrderBy(t => t.Index))
            {
                var team = new Team(teamEntity);
                Teams.Add(team);
            }
        }
    }
}
