﻿using System;
using TDS.Server.Database.Interfaces;

namespace TDS.Server.Database.Entity.Player
{
    public partial class PlayerStats : IPlayerDataTable
    {
        public DateTime? LastFreeUsernameChange { get; set; }
        public DateTime LastLoginTimestamp { get; set; }
        public DateTime LastMapsBoughtCounterReduce { get; set; }
        public bool LoggedIn { get; set; }
        public int MapsBoughtCounter { get; set; }
        public int Money { get; set; }
        public int? MuteTime { get; set; }
        public virtual Players Player { get; set; }
        public int PlayerId { get; set; }
        public int PlayTime { get; set; }
        public int? VoiceMuteTime { get; set; }
        public int AmountLowPriorityIssues { get; set; }
        public int AmountMediumPriorityIssues { get; set; }
        public int AmountHighPriorityIssues { get; set; }
        public int AmountUrgentPriorityIssues { get; set; }
    }
}
