using System;

namespace TDS_Server_DB.Entity.Rest
{
    public class Announcements
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string Text { get; set; }
    }
}
