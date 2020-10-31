using TDS_Server.Data.Interfaces;

namespace TDS_Server.Data.Interfaces.GangsSystem
{
#nullable enable
    public interface IGangHouseHandler
    {
        IGangHouse? House { get; set; }
    }
}