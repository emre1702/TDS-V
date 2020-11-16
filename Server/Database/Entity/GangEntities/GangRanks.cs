using Newtonsoft.Json;
using System.Collections.Generic;

namespace TDS.Server.Database.Entity.GangEntities
{
    public class GangRanks
    {
        #region Public Properties

        [JsonIgnore]
        public virtual Gangs Gang { get; set; }
        [JsonIgnore]
        public int GangId { get; set; }
        [JsonProperty("0")]
        public string Name { get; set; }
        [JsonIgnore]
        public short Rank { get; set; }
        [JsonProperty("1")]
        public string Color { get; set; }
        [JsonProperty("2")]
        public int Id { get; set; }

        [JsonIgnore]
        public virtual ICollection<GangMembers> GangMembers { get; internal set; }

        #endregion Public Properties
    }
}
