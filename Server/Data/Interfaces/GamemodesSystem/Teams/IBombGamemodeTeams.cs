namespace TDS_Server.Data.Interfaces.GamemodesSystem.Teams
{
    public interface IBombGamemodeTeams : IBaseGamemodeTeams
    {
        ITeam CounterTerrorists { get; }
        ITeam Terrorists { get; }
    }
}
