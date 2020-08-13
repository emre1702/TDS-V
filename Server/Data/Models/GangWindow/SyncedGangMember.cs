using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using System;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Database.Entity.GangEntities;

namespace TDS_Server.Data.Models.GangWindow
{
    public class SyncedGangMember
    {
        [JsonProperty("0")]
        public int PlayerId { get; set; }

        [JsonProperty("1")]
        public string Name { get; set; }

        [JsonProperty("2")]
        public string JoinDate { get; set; }

        [JsonProperty("3")]
        public string LastLoginDate { get; set; }

        [JsonProperty("4")]
        public bool IsOnline { get; set; }

        [JsonProperty("5")]
        public short Rank { get; set; }

        [JsonProperty("6")]
        public int JoinDateSortNumber { get; set; }

        [JsonProperty("7")]
        public int LastLoginSortNumber { get; set; }


        public SyncedGangMember(ITDSPlayer forPlayer, GangMembers copyFrom)
        {
            PlayerId = copyFrom.PlayerId;
            Name = copyFrom.Name;
            JoinDate = forPlayer.GetLocalDateTimeString(copyFrom.JoinTime);
            LastLoginDate = forPlayer.GetLocalDateTimeString(copyFrom.LastLogin);
            IsOnline = forPlayer.Gang.PlayersOnline.Any(p => p.Entity.Id == copyFrom.PlayerId);
            Rank = copyFrom.Rank.Rank;
            JoinDateSortNumber = (int)(DateTime.UtcNow - copyFrom.JoinTime).TotalSeconds;
            LastLoginSortNumber = (int)(DateTime.UtcNow - copyFrom.LastLogin).TotalSeconds;
        }
    }
}
