using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Database.Entity.GangEntities;

namespace TDS.Server.Data.Interfaces.GangsSystem
{
#nullable enable
    public interface IGangsProvider
    {
        IGang GetGang(Gangs entity);
    }
}