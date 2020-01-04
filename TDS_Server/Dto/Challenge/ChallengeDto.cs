using MessagePack;
using TDS_Common.Enum.Challenge;

namespace TDS_Server.Dto.Challlenge
{
    [MessagePackObject]
    public class ChallengeDto
    {
        [Key(0)]
        public EChallengeType Type { get; set; }

        [Key(1)]
        public int Amount { get; set; }

        [Key(2)]
        public int CurrentAmount { get; set; }
    }
}
