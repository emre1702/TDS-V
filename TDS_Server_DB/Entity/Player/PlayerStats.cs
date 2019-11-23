using System;

namespace TDS_Server_DB.Entity.Player
{
    public partial class PlayerStats
    {
        public int PlayerId { get; set; }
        public int Money { get; set; }
        public int PlayTime { get; set; }
        public int? MuteTime { get; set; }
        public int? VoiceMuteTime { get; set; }
        public bool LoggedIn { get; set; }
        public DateTime LastLoginTimestamp { get; set; }
        public int MapsBoughtCounter { get; set; }
        public DateTime LastMapsBoughtCounterReduce { get; set; }

        public virtual Players Player { get; set; }
    }
}
