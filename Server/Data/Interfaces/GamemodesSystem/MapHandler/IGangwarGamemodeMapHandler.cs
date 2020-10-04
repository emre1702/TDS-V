using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.Entities.Gangs;

namespace TDS_Server.Data.Interfaces.GamemodesSystem.MapHandler
{
#nullable enable

    public interface IGangwarGamemodeMapHandler
    {
        IGangwarArea Area { get; }
        ITDSObject? TargetObject { get; }
    }
}
