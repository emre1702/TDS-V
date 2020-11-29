using System.Collections.Generic;

namespace TDS.Server.Data.Models
{
    public class CommandMethodAndArgs
    {
        public CommandMethodDataDto Method { get; set; }
        public List<object> Args { get; set; } 
        public string RemainingText { get; set; }
    }
}
