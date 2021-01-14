using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS.Shared.Data.Interfaces.Map.Creator;

namespace TDS.Client.Data.Extensions
{
    public static class LocationDataExtensions
    {
        public static void LoadLocation(this IMapLocationData location)
        {
            location.Ipls?.ForEach(ipl => LoadIpl(ipl));
            location.IplsToUnload?.ForEach(ipl => UnloadIpl(ipl));
            CallHashDatas(location.Name, true);
        }

        public static void UnloadLocation(this IMapLocationData location)
        {
            location.Ipls?.ForEach(ipl => UnloadIpl(ipl));
            location.IplsToUnload?.ForEach(ipl => LoadIpl(ipl));
            CallHashDatas(location.Name, false);
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

        private static void CallHashDatas(string locationName, bool toggle)
        {
            switch (locationName.ToLower())
            {
                case "cayo pericio heist island":
                    Invoker.Invoke(0x9A9D1BA639675CF1, "HeistIsland", toggle ? 1 : 0);
                    Invoker.Invoke(0x5E1460624D194A38, toggle ? 1 : 0);
                    break;
            }
        }
    }
}