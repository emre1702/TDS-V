using TDS_Client.Data.Interfaces.ModAPI.Stats;

namespace TDS_Client.RAGEAPI.Stats
{
    internal class StatsAPI : IStatsAPI
    {
        #region Public Methods

        public void StatSetInt(uint statName, int value, bool save)
        {
            RAGE.Game.Stats.StatSetInt(statName, value, save);
        }

        #endregion Public Methods
    }
}
