using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Data.Interfaces.TeamsSystem;

namespace TDS.Server.GangsSystem
{
    public class TeamHandler : IGangTeamHandler
    {
#nullable disable
        public ITeam GangLobbyTeam { get; set; }
#nullable restore
    }
}
