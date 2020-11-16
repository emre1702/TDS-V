using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using System;
using System.Linq;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Database.Entity.GangEntities;

namespace TDS.Server.Data.Models.GangWindow
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
            JoinDate = forPlayer.Timezone.GetLocalDateTimeString(copyFrom.JoinTime);
            LastLoginDate = forPlayer.Timezone.GetLocalDateTimeString(copyFrom.LastLogin);
            IsOnline = forPlayer.Gang.Players.GetOnline(forPlayer.Id) is { };
            Rank = copyFrom.Rank.Rank;
            JoinDateSortNumber = (int)(DateTime.UtcNow - copyFrom.JoinTime).TotalSeconds;
            LastLoginSortNumber = (int)(DateTime.UtcNow - copyFrom.LastLogin).TotalSeconds;
        }
    }
}
