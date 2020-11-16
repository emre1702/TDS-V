using TDS.Shared.Data.Enums;

namespace TDS.Server.Database.Entity.Userpanel
{
    public class FAQs
    {
        #region Public Properties

        public string Answer { get; set; }
        public int Id { get; set; }
        public Language Language { get; set; }
        public string Question { get; set; }

        #endregion Public Properties
    }
}
