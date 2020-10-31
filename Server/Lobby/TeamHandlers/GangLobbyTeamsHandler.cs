using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Handler.GangSystem;
using TDS_Server.LobbySystem.Lobbies;

namespace TDS_Server.LobbySystem.TeamHandlers
{
    public class GangLobbyTeamsHandler : BaseLobbyTeamsHandler
    {
        private readonly GangsHandler _gangsHandler;

        public GangLobbyTeamsHandler(GangLobby lobby, IBaseLobbyEventsHandler events, GangsHandler gangsHandler, ITeamsProvider teamsProvider)
            : base(lobby, events, teamsProvider)
        {
            _gangsHandler = gangsHandler;
            LoadGangTeams();
        }

        protected override ValueTask Events_PlayerJoined((ITDSPlayer Player, int TeamIndex) data)
        {
            SetPlayerTeam(data.Player, data.Player.Gang.TeamHandler.GangLobbyTeam);
            return default;
        }

        private void LoadGangTeams()
        {
            Do(teams =>
            {
                foreach (var team in teams)
                {
                    var teamId = team.Entity.Id;
                    var gang = _gangsHandler.GetByTeamId(teamId);
                    if (gang != null)
                    {
                        gang.TeamHandler.GangLobbyTeam = team;
                    }
                }
            }).Wait();
        }
    }
}
