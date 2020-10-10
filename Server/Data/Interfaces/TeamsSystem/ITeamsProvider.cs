using TDS_Server.Database.Entity.Rest;

namespace TDS_Server.Data.Interfaces.TeamsSystem
{
    public interface ITeamsProvider
    {
        ITeam Create(Teams entity);
    }
}
