using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces.GamemodesSystem.Teams;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.TeamsSystem;

namespace TDS.Server.GamemodesSystem.Teams
{
    public class BombTeams : BaseGamemodeTeams, IBombGamemodeTeams
    {
        public ITeam Terrorists { get; }
        public ITeam CounterTerrorists { get; }

        public BombTeams(IRoundFightLobby lobby)
        {
            var teams = lobby.Teams.GetTeams();
            Terrorists = teams[(int)GamemodeTeamTypeIndex.Terrorists];
            CounterTerrorists = teams[(int)GamemodeTeamTypeIndex.CounterTerrorists];
        }
    }
}
