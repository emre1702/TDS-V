using GTANetworkAPI;

namespace TDS.Server.Data.Interfaces.GangsSystem
{
#nullable enable
    public interface IGangMapHandler
    {
        float? SpawnHeading { get; }
        Vector3? SpawnPosition { get; }
    }
}