namespace TDS_Client.Data.Interfaces.ModAPI.Stats
{
    public interface IStatsAPI
    {
        void StatSetInt(uint statName, int value, bool save);
    }
}
