using TDS_Server.Data.Interfaces.GangsSystem;
using TDS_Server.Data.Interfaces.TeamsSystem;

namespace TDS_Server.GangsSystem
{
    public class TeamHandler : IGangTeamHandler
    {
#nullable disable
        public ITeam GangLobbyTeam { get; set; }
#nullable restore
    }
}
