using System.Collections.Generic;
using TDS_Shared.Data.Enums.Userpanel;

namespace TDS_Server.Database.Entity.Userpanel
{
    public class Rules
    {
        #region Public Constructors

        public Rules()
        {
            RuleTexts = new HashSet<RuleTexts>();
        }

        #endregion Public Constructors

        #region Public Properties

        public RuleCategory Category { get; set; }
        public int Id { get; set; }

        public virtual ICollection<RuleTexts> RuleTexts { get; set; }
        public RuleTarget Target { get; set; }

        #endregion Public Properties
    }
}
