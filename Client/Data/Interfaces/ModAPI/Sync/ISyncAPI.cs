namespace TDS_Client.Data.Interfaces.ModAPI.Sync
{
    public interface ISyncAPI
    {
        void SendEvent(string eventName, params object[] args);
    }
}
