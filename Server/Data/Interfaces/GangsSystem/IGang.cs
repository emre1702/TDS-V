using System.Threading.Tasks;
using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Database.Entity.GangEntities;

namespace TDS.Server.Data.Interfaces.GangsSystem
{
#nullable enable
    public interface IGang
    {
        IGangActionHandler Action { get; }
        IGangChat Chat { get; set; }
        IDatabaseHandler Database { get; }
        IGangHouseHandler HouseHandler { get; }
        IGangInvitations Invitations { get; }
        IGangLeaderHandler LeaderHandler { get; set; }
        IGangMapHandler MapHandler { get; }
        IGangPermissionsHandler PermissionsHandler { get; }
        IGangPlayers Players { get; }
        IGangTeamHandler TeamHandler { get; }

        bool Initialized { get; set; }
        bool Deleted { get; }
        Gangs Entity { get; }

        Task Delete();
        void Init(Gangs entity);
    }
}