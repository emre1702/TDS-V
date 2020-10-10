using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.GamemodesSystem.MapHandler
{
#nullable enable

    public interface IGangwarGamemodeMapHandler
    {
        ITDSObject? TargetObject { get; }
    }
}
