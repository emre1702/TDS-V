using Newtonsoft.Json;

namespace TDS_Shared.Data.Models
{
    public class SyncedGangPermissions
    {
        [JsonProperty("0")]
        public ushort InviteMembers { get; set; }

        [JsonProperty("1")]
        public ushort KickMembers { get; set; }

        [JsonProperty("2")]
        public ushort ManagePermissions { get; set; }

        [JsonProperty("3")]
        public ushort ManageRanks { get; set; }

        [JsonProperty("4")]
        public ushort StartGangwar { get; set; }

        [JsonProperty("5")]
        public ushort SetRanks { get; set; }
    }
}
