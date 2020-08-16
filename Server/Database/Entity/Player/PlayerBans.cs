using System;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Database.Entity.Player
{
    public partial class PlayerBans
    {
        #region Public Properties

        public long? __HwId { get; set; }
        public long? __HwIdEx { get; set; }

        public ulong? HwId
        {
            get { unchecked { return (ulong)__HwId; } }
            set { unchecked { __HwId = (long)value; } }
        }

        public ulong? HwIdEx
        {
            get { unchecked { return (ulong)__HwIdEx; } }
            set { unchecked { __HwIdEx = (long)value; } }
        }

        public virtual Players Admin { get; set; }
        public int? AdminId { get; set; }
        public DateTime? EndTimestamp { get; set; }
        public string IP { get; set; }
        public virtual Lobbies Lobby { get; set; }
        public int LobbyId { get; set; }
        public virtual Players Player { get; set; }
        public int PlayerId { get; set; }
        public bool PreventConnection { get; set; }
        public string Reason { get; set; }
        public ulong? SCId { get; set; }
        public DateTime StartTimestamp { get; set; }

        #endregion Public Properties
    }
}
