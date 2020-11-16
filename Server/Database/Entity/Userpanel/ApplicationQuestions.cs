using System.Collections.Generic;
using TDS.Server.Database.Entity.Player;
using TDS.Shared.Data.Enums.Userpanel;

namespace TDS.Server.Database.Entity.Userpanel
{
    public class ApplicationQuestions
    {
        #region Public Properties

        public virtual Players Admin { get; set; }
        public int AdminId { get; set; }
        public virtual ICollection<ApplicationAnswers> Answers { get; set; }
        public UserpanelAdminQuestionAnswerType AnswerType { get; set; }
        public int Id { get; set; }
        public string Question { get; set; }

        #endregion Public Properties
    }
}
