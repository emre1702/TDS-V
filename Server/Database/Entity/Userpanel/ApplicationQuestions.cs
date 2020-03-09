using System;
using System.Collections.Generic;
using System.Text;
using TDS_Common.Enum.Userpanel;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Database.Entity.Userpanel
{
    public class ApplicationQuestions
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public string Question { get; set; }
        public EUserpanelAdminQuestionAnswerType AnswerType { get; set; }

        public virtual Players Admin { get; set; }
        public virtual ICollection<ApplicationAnswers> Answers { get; set; }
    }
}
