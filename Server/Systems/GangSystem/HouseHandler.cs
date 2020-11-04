using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.GangsSystem;

namespace TDS_Server.GangsSystem
{
    public class HouseHandler : IGangHouseHandler
    {
        //Todo: Don't forget to use this when buying, selling or losing the house
        public IGangHouse? House { get; set; }
    }
}
