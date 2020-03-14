using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Database.Entity.Userpanel
{
    public class ApplicationAnswers
    {
        public int ApplicationId { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }

        public virtual Applications Application { get; set; }
        public virtual ApplicationQuestions Question { get; set; }
    }
}
