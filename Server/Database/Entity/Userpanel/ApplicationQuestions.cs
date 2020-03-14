﻿using System.Collections.Generic;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums.Userpanel;

namespace TDS_Server.Database.Entity.Userpanel
{
    public class ApplicationQuestions
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public string Question { get; set; }
        public UserpanelAdminQuestionAnswerType AnswerType { get; set; }

        public virtual Players Admin { get; set; }
        public virtual ICollection<ApplicationAnswers> Answers { get; set; }
    }
}
