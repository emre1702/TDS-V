using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Database.Entity.GangEntities;

namespace TDS.Server.Data.Interfaces.GangActionAreaSystem.Database
{
    #nullable enable
    public interface IBaseGangActionAreaDatabase
    {
        IDatabaseHandler Database { get; set; }
        GangActionAreas? Entity { get; }

        void Init(GangActionAreas entity);
    }
}