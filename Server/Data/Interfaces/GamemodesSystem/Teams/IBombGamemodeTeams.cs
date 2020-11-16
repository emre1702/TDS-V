using TDS.Server.Data.Interfaces.TeamsSystem;

namespace TDS.Server.Data.Interfaces.GamemodesSystem.Teams
{
    public interface IBombGamemodeTeams : IBaseGamemodeTeams
    {
        ITeam CounterTerrorists { get; }
        ITeam Terrorists { get; }
    }
}
