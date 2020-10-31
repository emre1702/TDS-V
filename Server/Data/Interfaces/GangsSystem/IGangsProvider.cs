using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Database.Entity.GangEntities;

namespace TDS_Server.Data.Interfaces.GangsSystem
{
#nullable enable
    public interface IGangsProvider
    {
        IGang Get(Gangs entity);
    }
}