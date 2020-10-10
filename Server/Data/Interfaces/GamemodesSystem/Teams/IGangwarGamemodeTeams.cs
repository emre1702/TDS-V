using TDS_Server.Data.Interfaces.TeamsSystem;

namespace TDS_Server.Data.Interfaces.GamemodesSystem.Teams
{
#nullable enable

    public interface IGangwarGamemodeTeams : IBaseGamemodeTeams
    {
        ITeam Attacker { get; }
        ITeam Owner { get; }
    }
}
