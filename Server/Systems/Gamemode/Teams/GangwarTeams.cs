using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces.GamemodesSystem.Teams;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.TeamsSystem;

namespace TDS.Server.GamemodesSystem.Teams
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
