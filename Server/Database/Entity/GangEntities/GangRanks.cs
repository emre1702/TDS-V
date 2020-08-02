using Newtonsoft.Json;

namespace TDS_Server.Database.Entity.GangEntities
{
    public class GangRanks
    {
        #region Public Properties

        [JsonIgnore]
        public virtual Gangs Gang { get; set; }
        [JsonIgnore]
        public int GangId { get; set; }
        [JsonProperty("1")]
        public string Name { get; set; }
        [JsonProperty("0")]
        public short Rank { get; set; }
        [JsonProperty("2")]
        public string Color { get; set; }

        [JsonProperty("99")]
        public short? OriginalRank { get; set; }

        #endregion Public Properties
    }
}
