using MessagePack;
using System.Collections.Generic;
using TDS_Common.Enum.Challenge;

namespace TDS_Server.Dto.Challlenge
{
    [MessagePackObject]
    public class ChallengeGroupDto
    {
        [Key(0)]
        public EChallengeFrequency Frequency { get; set; }
        [Key(1)]
        public IEnumerable<ChallengeDto> Challenges { get; set; } = new List<ChallengeDto>();
    }
}
