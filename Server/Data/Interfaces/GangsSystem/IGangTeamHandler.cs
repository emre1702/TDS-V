using TDS.Server.Data.Interfaces.TeamsSystem;

namespace TDS.Server.Data.Interfaces.GangsSystem
{
#nullable enable
    public interface IGangTeamHandler
    {
        ITeam GangLobbyTeam { get; set; }
    }
}