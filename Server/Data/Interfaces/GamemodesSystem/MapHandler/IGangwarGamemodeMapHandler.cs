using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.GamemodesSystem.MapHandler
{
#nullable enable

    public interface IGangwarGamemodeMapHandler
    {
        ITDSObject? TargetObject { get; }
    }
}
