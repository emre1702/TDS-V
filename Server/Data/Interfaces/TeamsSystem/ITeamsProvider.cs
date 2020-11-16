using TDS.Server.Database.Entity.Rest;

namespace TDS.Server.Data.Interfaces.TeamsSystem
{
    public interface ITeamsProvider
    {
        ITeam Create(Teams entity);
    }
}
