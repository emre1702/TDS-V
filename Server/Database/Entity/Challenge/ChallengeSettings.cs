using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Entity.Challenge
{
    public class ChallengeSettings
    {
        public ChallengeType Type { get; set; }
        public ChallengeFrequency Frequency { get; set; }
        public int MinNumber { get; set; }
        public int MaxNumber { get; set; }
    
    }
}
