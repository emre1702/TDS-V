using Newtonsoft.Json;
using TDS_Common.Enum.Challenge;

namespace TDS_Server.Dto.Challlenge
{
    public class ChallengeModel
    {
        [JsonProperty("0")]
        public EChallengeType Type { get; set; }

        [JsonProperty("1")]
        public int Amount { get; set; }

        [JsonProperty("2")]
        public int CurrentAmount { get; set; }
    }
}
