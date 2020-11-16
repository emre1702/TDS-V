using Newtonsoft.Json;
using System.Collections.Generic;
using TDS.Shared.Data.Enums.Userpanel;

namespace TDS.Server.Data.Models.Userpanel.Rules
{
    public class RuleData
    {
        [JsonProperty("3")]
        public RuleCategory Category { get; set; }

        [JsonProperty("0")]
        public int Id { get; set; }

        [JsonProperty("2")]
        public RuleTarget Target { get; set; }

        [JsonProperty("1")]
        public Dictionary<int, string> Texts { get; set; }
    }
}
