using Newtonsoft.Json;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Data.Models.Userpanel.Stats
{
    public class UserpanelPlayerWeaponBodyPartStatsData
    {

        [JsonProperty("5")]
        public int AmountHits { get; set; }

        [JsonProperty("6")]
        public int AmountOfficialHits { get; set; }

        [JsonProperty("0")]
        public PedBodyPart BodyPart { get; set; }

        [JsonProperty("3")]
        public long DealtDamage { get; set; }

        [JsonProperty("4")]
        public long DealtOfficialDamage { get; set; }

        [JsonProperty("1")]
        public int Kills { get; set; }

        [JsonProperty("2")]
        public int OfficialKills { get; set; }

    }
}
