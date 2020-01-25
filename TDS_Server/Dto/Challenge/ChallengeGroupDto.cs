using Newtonsoft.Json;
using System.Collections.Generic;
using TDS_Common.Enum.Challenge;

namespace TDS_Server.Dto.Challlenge
{
    public class ChallengeGroupDto
    {
        [JsonProperty("0")]
        public EChallengeFrequency Frequency { get; set; }
        [JsonProperty("1")]
        public IEnumerable<ChallengeDto> Challenges { get; set; } = new List<ChallengeDto>();
    }
}
