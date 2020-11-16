using TDS.Server.Data.Interfaces.GangsSystem;

namespace TDS.Server.GangsSystem
{
    public class ActionHandler : IGangActionHandler
    {
        public bool InAction { get; set; }
    }
}
