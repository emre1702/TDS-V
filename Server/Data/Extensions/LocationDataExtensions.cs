using TDS.Server.Data.Models.Map;
using TDS.Shared.Data.Models.Map.Creator;

namespace TDS.Server.Data.Extensions
{
    public static class LocationDataExtensions
    {
        public static MapSharedLocation SwitchNamespace(this MapSharedLocationXml location)
            => new MapSharedLocation
            {
                Name = location.Name,
                Ipls = location.Ipls,
                IplsToUnload = location.IplsToUnload,
            };
    }
}