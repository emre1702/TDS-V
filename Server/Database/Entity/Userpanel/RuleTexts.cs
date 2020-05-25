using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.Entity.Userpanel
{
    public class RuleTexts
    {
        #region Public Properties

        public Language Language { get; set; }
        public virtual Rules Rule { get; set; }
        public int RuleId { get; set; }
        public string RuleStr { get; set; }

        #endregion Public Properties
    }
}
