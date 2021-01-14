using System.Collections.Generic;

namespace TDS.Shared.Data.Interfaces.Map.Creator
{
    public interface IMapLocationData
    {
        List<string> Ipls { get; set; }

        List<string> IplsToUnload { get; set; }

        ulong? Hash { get; set; }
    }
}