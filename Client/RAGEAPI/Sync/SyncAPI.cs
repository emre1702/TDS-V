using TDS_Client.Data.Interfaces.ModAPI.Sync;

namespace TDS_Client.RAGEAPI.Sync
{
    class SyncAPI : ISyncAPI
    {
        public void SendEvent(string eventName, params object[] args)
        {
            RAGE.Events.CallRemote(eventName, args);
        }
    }
}
