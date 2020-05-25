namespace TDS_Server.Database.Entity.Userpanel
{
    public class ApplicationAnswers
    {
        #region Public Properties

        public string Answer { get; set; }
        public virtual Applications Application { get; set; }
        public int ApplicationId { get; set; }
        public virtual ApplicationQuestions Question { get; set; }
        public int QuestionId { get; set; }

        #endregion Public Properties
    }
}
