using Newtonsoft.Json;
using System.Collections.Generic;
using TDS.Shared.Data.Enums.Challenge;

namespace TDS.Server.Data.Models.Challenge
{
    public class ChallengeGroupModel
    {
        [JsonProperty("1")]
        public IEnumerable<ChallengeModel> Challenges { get; set; } = new List<ChallengeModel>();

        [JsonProperty("0")]
        public ChallengeFrequency Frequency { get; set; }

    }
}
