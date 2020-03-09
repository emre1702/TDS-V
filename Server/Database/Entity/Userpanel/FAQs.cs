using TDS_Common.Enum;

namespace TDS_Server.Database.Entity.Userpanel
{
    public class FAQs
    {
        public int Id { get; set; }
        public ELanguage Language { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
