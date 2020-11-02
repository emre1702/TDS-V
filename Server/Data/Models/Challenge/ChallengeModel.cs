using Newtonsoft.Json;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Data.Models.Challenge
{
    public class ChallengeModel
    {
        [JsonProperty("1")]
        public int Amount { get; set; }

        [JsonProperty("2")]
        public int CurrentAmount { get; set; }

        [JsonProperty("0")]
        public ChallengeType Type { get; set; }
    }
}
