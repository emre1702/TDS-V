using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Handler.GangSystem;
using TDS.Server.LobbySystem.Lobbies;

namespace TDS.Server.LobbySystem.TeamHandlers
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

        protected override ValueTask OnPlayerJoined((ITDSPlayer Player, int TeamIndex) data)
        {
            SetPlayerTeam(data.Player, data.Player.Gang.TeamHandler.GangLobbyTeam);
            return default;
        }

        private void LoadGangTeams()
        {
            DoForList(teams =>
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
