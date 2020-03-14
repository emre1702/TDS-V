using System.Collections.Generic;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.Entity.Userpanel
{
    public class RuleTexts
    {
        public int RuleId { get; set; }
        public string RuleStr { get; set; }
        public Language Language { get; set; }

        public virtual Rules Rule { get; set; }
    }
}
