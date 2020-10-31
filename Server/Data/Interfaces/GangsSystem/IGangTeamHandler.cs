using TDS_Server.Data.Interfaces.TeamsSystem;

namespace TDS_Server.Data.Interfaces.GangsSystem
{
#nullable enable
    public interface IGangTeamHandler
    {
        ITeam GangLobbyTeam { get; set; }
    }
}