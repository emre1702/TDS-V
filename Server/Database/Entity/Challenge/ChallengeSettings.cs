using TDS_Shared.Data.Enums.Challenge;

namespace TDS_Server.Database.Entity.Challenge
{
    public class ChallengeSettings
    {
        #region Public Properties

        public ChallengeFrequency Frequency { get; set; }
        public int MaxNumber { get; set; }
        public int MinNumber { get; set; }
        public ChallengeType Type { get; set; }

        #endregion Public Properties
    }
}
