using System.Collections.Generic;

namespace TDS.Shared.Data.Interfaces.Map.Creator
{
    public interface IMapLocationData
    {
        string Name { get; set; }
        List<string> Ipls { get; set; }

        List<string> IplsToUnload { get; set; }
    }
}