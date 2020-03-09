using Newtonsoft.Json;
using System.Collections.Generic;
using TDS_Common.Enum.Challenge;

namespace TDS_Server.Dto.Challlenge
{
    public class ChallengeGroupModel
    {
        [JsonProperty("0")]
        public EChallengeFrequency Frequency { get; set; }
        [JsonProperty("1")]
        public IEnumerable<ChallengeModel> Challenges { get; set; } = new List<ChallengeModel>();
    }
}
