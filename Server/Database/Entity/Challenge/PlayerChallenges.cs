using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Entity.Challenge
{
    public class PlayerChallenges
    {
        public int PlayerId { get; set; }
        public ChallengeType Challenge { get; set; }
        public ChallengeFrequency Frequency { get; set; }
        public int Amount { get; set; }
        public int CurrentAmount { get; set; }

        public virtual Players Player { get; set; }
    }
}
