using TDS_Common.Enum.Challenge;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server_DB.Entity.Challenge
{
    public class PlayerChallenges
    {
        public int PlayerId { get; set; }
        public EChallengeType Challenge { get; set; }
        public EChallengeFrequency Frequency { get; set; }
        public int Amount { get; set; }
        public int CurrentAmount { get; set; }

        public virtual Players Player { get; set; }
    }
}
