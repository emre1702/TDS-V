using RAGE.Game;
using TDS.Shared.Data.Interfaces.Map.Creator;

namespace TDS.Client.Data.Extensions
{
    public static class LocationDataExtensions
    {
        public static void LoadLocation(this IMapLocationData location)
        {
            location.Ipls?.ForEach(ipl => LoadIpl(ipl));
            location.IplsToUnload?.ForEach(ipl => UnloadIpl(ipl));
            if (location.Hash.HasValue)
                Invoker.Invoke(location.Hash.Value, true);
        }

        public static void UnloadLocation(this IMapLocationData location)
        {
            location.Ipls?.ForEach(ipl => UnloadIpl(ipl));
            location.IplsToUnload?.ForEach(ipl => LoadIpl(ipl));
            if (location.Hash.HasValue)
                Invoker.Invoke(location.Hash.Value, false);
        }

        private static void LoadIpl(string iplName)
        {
            if (!Streaming.IsIplActive(iplName))
                Streaming.RequestIpl(iplName);
        }

        private static void UnloadIpl(string iplName)
        {
            if (Streaming.IsIplActive(iplName))
                Streaming.RemoveIpl(iplName);
        }
    }
}