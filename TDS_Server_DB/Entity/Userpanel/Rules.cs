using System.Collections.Generic;
using TDS_Common.Enum;

namespace TDS_Server_DB.Entity.Userpanel
{
    public class Rules
    {
        public int Id { get; set; }

        public ERuleTarget Target { get; set; }

        public ERuleCategory Category { get; set; }

        public virtual ICollection<RuleTexts> RuleTexts { get; set; }

        public Rules()
        {
            RuleTexts = new HashSet<RuleTexts>();
        }
    }
}
