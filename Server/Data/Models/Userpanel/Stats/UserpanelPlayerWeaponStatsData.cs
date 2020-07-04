using Newtonsoft.Json;
using System.Collections.Generic;

namespace TDS_Server.Data.Models.Userpanel.Stats
{
    public class UserpanelPlayerWeaponStatsData
    {
        #region Public Properties

        [JsonProperty("10")]
        public int AmountHeadshots { get; set; }

        [JsonProperty("8")]
        public int AmountHits { get; set; }

        [JsonProperty("11")]
        public int AmountOfficialHeadshots { get; set; }

        [JsonProperty("9")]
        public int AmountOfficialHits { get; set; }

        [JsonProperty("7")]
        public int AmountOfficialShots { get; set; }

        [JsonProperty("6")]
        public int AmountShots { get; set; }

        [JsonProperty("1")]
        public List<UserpanelPlayerWeaponBodyPartStatsData> BodyPartsStats { get; set; }

        [JsonProperty("4")]
        public long DealtDamage { get; set; }

        [JsonProperty("5")]
        public long DealtOfficialDamage { get; set; }

        [JsonProperty("2")]
        public int Kills { get; set; }

        [JsonProperty("3")]
        public int OfficialKills { get; set; }

        [JsonProperty("0")]
        public string WeaponName { get; set; }

        #endregion Public Properties
    }
}
