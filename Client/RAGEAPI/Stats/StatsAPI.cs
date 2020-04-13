using TDS_Client.Data.Interfaces.ModAPI.Stats;

namespace TDS_Client.RAGEAPI.Stats
{
    class StatsAPI : IStatsAPI
    {
        public void StatSetInt(uint statName, int value, bool save)
        {
            RAGE.Game.Stats.StatSetInt(statName, value, save);
        }
    }
}
