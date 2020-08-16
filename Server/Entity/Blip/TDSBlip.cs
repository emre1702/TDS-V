using AltV.Net.Data;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Entity.Blip
{
    public class TDSBlip : ITDSBlip
    {
        //Todo: Implement TDSBlip
        public TDSBlip(uint sprite, Position position, float scale = 1f, byte color = 0, string name = "", byte alpha = 255, float drawDistance = 0, 
            bool shortRange = false, int dimension = 0)
        {
            Color = color;
            Name = name;
        }

        public byte Color { get; set; }
        public string Name { get; set; }

        public void Delete()
        {

        }
    }
}
