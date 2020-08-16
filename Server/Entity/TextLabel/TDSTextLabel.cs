using AltV.Net.Data;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Models;

namespace TDS_Server.Entity.TextLabel
{
    //Todo: Implement this
    public class TDSTextLabel : ITDSTextLabel
    {

        public TDSTextLabel(string text, Position position, double range, float fontSize, int font, Color color, bool entitySeethrough, int dimension)
        {

        }

        public string Text { get => ""; set => _ = ""; }

        public void Delete()
        {
            
        }
    }
}
