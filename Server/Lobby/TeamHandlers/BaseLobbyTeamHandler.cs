using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Entities.TeamSystem;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.TeamHandlers
{
    public class BaseLobbyTeamHandler
    {
        public List<ITeam> Teams { get; set; }

        public BaseLobbyTeamHandler(LobbyDb entity)
        {
            Teams = new List<ITeam>(entity.Teams.Count);
            foreach (var teamEntity in entity.Teams.OrderBy(t => t.Index))
            {
                var team = new Team(teamEntity);
                Teams.Add(team);
            }
        }
    }
}
