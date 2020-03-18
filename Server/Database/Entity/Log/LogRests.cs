﻿using System;
using System.Net;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.Entity.Log
{
    public partial class LogRests
    {
        public long Id { get; set; }
        public LogType Type { get; set; }
        public int Source { get; set; }
        public string Serial { get; set; }
        public IPAddress Ip { get; set; }
        public int? Lobby { get; set; }
        public DateTime Timestamp { get; set; }
    }
}