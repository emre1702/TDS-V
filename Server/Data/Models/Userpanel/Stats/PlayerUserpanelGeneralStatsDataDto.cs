using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TDS_Server.Data.Models.Userpanel.History;

namespace TDS_Server.Data.Models.Userpanel.Stats
{
    public class PlayerUserpanelGeneralStatsDataDto
    {
        [JsonProperty("4")]
        public short AdminLvl { get; set; }

        [JsonProperty("13")]
        public int AmountMapsCreated { get; set; }

        [JsonProperty("16")]
        public int AmountMapsRated { get; set; }

        [JsonProperty("12")]
        public List<string> BansInLobbies { get; set; }

        [JsonProperty("15")]
        public double CreatedMapsAverageRating { get; set; }

        [JsonProperty("5")]
        public short Donation { get; set; }

        [JsonProperty("3")]
        public string Gang { get; set; }

        [JsonProperty("0")]
        public int Id { get; set; }

        [JsonProperty("6")]
        public bool IsVip { get; set; }

        [JsonProperty("17")]
        public string LastLogin { get; set; }

        [JsonIgnore]
        public DateTime LastLoginDateTime { get; set; }

        [JsonProperty("19")]
        public List<PlayerUserpanelLobbyStats> LobbyStats { get; set; }

        [JsonProperty("20")]
        public List<PlayerUserpanelAdminTargetHistoryDataDto> Logs { get; set; }

        [JsonProperty("14")]
        public double MapsRatedAverage { get; set; }

        [JsonProperty("7")]
        public int Money { get; set; }

        [JsonProperty("10")]
        public int? MuteTime { get; set; }

        [JsonProperty("1")]
        public string Name { get; set; }

        [JsonProperty("9")]
        public int PlayTime { get; set; }

        [JsonIgnore]
        public DateTime RegisterDateTime { get; set; }

        [JsonProperty("18")]
        public string RegisterTimestamp { get; set; }

        [JsonProperty("2")]
        public string SCName { get; set; }

        [JsonProperty("8")]
        public long TotalMoney { get; set; }

        [JsonProperty("11")]
        public int? VoiceMuteTime { get; set; }
    }
}
