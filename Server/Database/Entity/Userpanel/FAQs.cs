using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.Entity.Userpanel
{
    public class FAQs
    {
        public int Id { get; set; }
        public Language Language { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
