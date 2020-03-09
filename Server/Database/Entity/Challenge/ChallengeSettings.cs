using TDS_Common.Enum.Challenge;

namespace TDS_Server.Database.Entity.Challenge
{
    public class ChallengeSettings
    {
        public EChallengeType Type { get; set; }
        public EChallengeFrequency Frequency { get; set; }
        public int MinNumber { get; set; }
        public int MaxNumber { get; set; }
    
    }
}
