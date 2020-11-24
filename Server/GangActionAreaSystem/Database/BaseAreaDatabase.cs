using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Database;
using TDS.Server.Database.Entity.GangEntities;

namespace TDS.Server.GangActionAreaSystem.Database
{
    internal class BaseAreaDatabase : IBaseGangActionAreaDatabase
    {
        public GangActionAreas? Entity { get; private set; }
        public IDatabaseHandler Database { get; set; }

        public BaseAreaDatabase(IDatabaseHandler databaseHandler)
        {
            Database = databaseHandler;
        }

        public void Init(GangActionAreas entity)
        {
            Entity = entity;
            Database.ExecuteForDBWithoutWait(dbContext => dbContext.Attach(Entity));
        }
    }
}
