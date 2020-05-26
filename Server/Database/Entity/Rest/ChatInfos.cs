using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.Entity.Rest
{
    public class ChatInfos
    {
        #region Public Properties

        public int Id { get; set; }
        public Language Language { get; set; }
        public string Message { get; set; }

        #endregion Public Properties
    }
}
