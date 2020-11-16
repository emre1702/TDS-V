using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Models
{
    public class RoundPlayerRewardsData
    {
        public uint KillsReward { get; set; }
        public uint AssistsReward { get; set; }
        public uint DamageReward { get; set; }
        public string Message { get; set; }
    }
}
