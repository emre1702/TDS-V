using System.Collections.Generic;
using TDS_Common.Enum;

namespace TDS_Server.Database.Entity.Userpanel
{
    public class RuleTexts
    {
        public int RuleId { get; set; }
        public string RuleStr { get; set; }
        public ELanguage Language { get; set; }

        public virtual Rules Rule { get; set; }
    }
}
