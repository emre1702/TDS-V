using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.GangsSystem;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Sync;

namespace TDS_Server.GangsSystem
{
    public class Gang : IGang
    {
        public IGangActionHandler Action { get; }
        public IGangChat Chat { get; set; }
        public IDatabaseHandler Database { get; }
        public IGangHouseHandler HouseHandler { get; }
        public IGangLeaderHandler LeaderHandler { get; set; }
        public IGangMapHandler MapHandler { get; }
        public IGangPermissionsHandler PermissionsHandler { get; }
        public IGangPlayers Players { get; }
        public IGangTeamHandler TeamHandler { get; }

        public bool Initialized { get; set; }
        public bool Deleted { get; private set; }

#nullable disable
        public Gangs Entity { get; private set; }
#nullable restore

        public Gang(IDatabaseHandler databaseHandler, LangHelper langHelper, GangsHandler gangsHandler, LobbiesHandler lobbiesHandler, DataSyncHandler dataSyncHandler)
        {
            Action = new ActionHandler();
            Players = new Players(gangsHandler, lobbiesHandler, dataSyncHandler);
            Chat = new Chat(langHelper, Players);
            Database = databaseHandler;
            HouseHandler = new HouseHandler();
            LeaderHandler = new LeaderHandler(this);
            MapHandler = new MapHandler(HouseHandler);
            PermissionsHandler = new PermissionsHandler(this);
            TeamHandler = new TeamHandler();
        }

        public void Init(Gangs entity)
        {
            Entity = entity;

            Database.ExecuteForDBWithoutWait(dbContext => dbContext.Attach(entity));
        }

        public async Task Delete()
        {
            Deleted = true;
            await Players.RemoveAll();
            await Database.ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Gangs.Remove(Entity);
                await dbContext.SaveChangesAsync();
                await dbContext.DisposeAsync();
            });
        }
    }
}
