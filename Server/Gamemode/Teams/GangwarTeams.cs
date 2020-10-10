using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces.GamemodesSystem.Teams;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.TeamsSystem;

namespace TDS_Server.GamemodesSystem.Teams
{
    public class GangwarTeams : BaseGamemodeTeams, IGangwarGamemodeTeams
    {
        public ITeam Attacker { get; }
        public ITeam Owner { get; }

        public GangwarTeams(IRoundFightLobby lobby)
        {
            var teams = lobby.Teams.GetTeams();
            Attacker = teams[(int)GangActionLobbyTeamIndex.Attacker];
            Owner = teams[(int)GangActionLobbyTeamIndex.Owner];
        }
    }
}
