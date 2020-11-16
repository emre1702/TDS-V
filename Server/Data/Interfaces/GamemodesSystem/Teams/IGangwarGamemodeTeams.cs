using TDS.Server.Data.Interfaces.TeamsSystem;

namespace TDS.Server.Data.Interfaces.GamemodesSystem.Teams
{
#nullable enable

    public interface IGangwarGamemodeTeams : IBaseGamemodeTeams
    {
        ITeam Attacker { get; }
        ITeam Owner { get; }
    }
}
