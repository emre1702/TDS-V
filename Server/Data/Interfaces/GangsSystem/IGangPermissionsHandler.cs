using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;

namespace TDS.Server.Data.Interfaces.GangsSystem
{
    public interface IGangPermissionsHandler
    {
        bool IsAllowedTo(ITDSPlayer player, GangCommand type);
    }
}