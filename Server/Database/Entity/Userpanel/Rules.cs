﻿using System.Collections.Generic;
using TDS.Shared.Data.Enums.Userpanel;

namespace TDS.Server.Database.Entity.Userpanel
{
    public class Rules
    {
        public Rules() => RuleTexts = new HashSet<RuleTexts>();

        public RuleCategory Category { get; set; }
        public int Id { get; set; }

        public virtual ICollection<RuleTexts> RuleTexts { get; set; }
        public RuleTarget Target { get; set; }
    }
}
