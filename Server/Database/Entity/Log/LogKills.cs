using System;

namespace TDS.Server.Database.Entity.Log
{
    public class LogKills
    {
        #region Public Properties

        public int DeadId { get; set; }
        public long Id { get; set; }
        public int KillerId { get; set; }
        public DateTime Timestamp { get; set; }
        public uint WeaponId { get; set; }

        #endregion Public Properties
    }
}
