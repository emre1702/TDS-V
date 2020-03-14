using System.Collections.Generic;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Userpanel;

namespace TDS_Server.Database.Entity.Userpanel
{
    public class Rules
    {
        public int Id { get; set; }

        public RuleTarget Target { get; set; }

        public RuleCategory Category { get; set; }

        public virtual ICollection<RuleTexts> RuleTexts { get; set; }

        public Rules()
        {
            RuleTexts = new HashSet<RuleTexts>();
        }
    }
}
