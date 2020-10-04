using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.GamemodesSystem.Teams;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Shared.Data.Utility;

namespace TDS_Server.GamemodesSystem.Teams
{
    public class BombTeams : BaseGamemodeTeams, IBombGamemodeTeams
    {
        public ITeam Terrorists { get; }
        public ITeam CounterTerrorists { get; }

        public BombTeams(IRoundFightLobby lobby)
        {
            var teams = lobby.Teams.GetTeams();
            var terroristsIndex = SharedUtils.GetRandom(1, 2);
            Terrorists = teams[terroristsIndex];
            CounterTerrorists = teams[terroristsIndex == 1 ? 2 : 1];
        }
    }
}
