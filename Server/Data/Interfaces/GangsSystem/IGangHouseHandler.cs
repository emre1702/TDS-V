using TDS.Server.Data.Interfaces;

namespace TDS.Server.Data.Interfaces.GangsSystem
{
#nullable enable
    public interface IGangHouseHandler
    {
        IGangHouse? House { get; set; }
    }
}