using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;

namespace TDS_Server.Data.Interfaces.GangsSystem
{
    public interface IGangPermissionsHandler
    {
        bool IsAllowedTo(ITDSPlayer player, GangCommand type);
    }
}