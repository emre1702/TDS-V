namespace TDS_Client.Data.Interfaces.ModAPI.Stats
{
    public interface IStatsAPI
    {
        #region Public Methods

        void StatSetInt(uint statName, int value, bool save);

        #endregion Public Methods
    }
}
