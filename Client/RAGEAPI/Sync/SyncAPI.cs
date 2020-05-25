using TDS_Client.Data.Interfaces.ModAPI.Sync;

namespace TDS_Client.RAGEAPI.Sync
{
    internal class SyncAPI : ISyncAPI
    {
        #region Public Methods

        public void SendEvent(string eventName, params object[] args)
        {
            RAGE.Events.CallRemote(eventName, args);
        }

        #endregion Public Methods
    }
}
