namespace TDS_Client.Data.Interfaces.ModAPI.Stats
{
    public interface IStatsAPI
    {
        void StatSetInt(uint stat, int value, bool save);
    }
}
