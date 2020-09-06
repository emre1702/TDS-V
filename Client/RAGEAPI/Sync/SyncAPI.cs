using TDS_Client.Data.Interfaces.RAGE.Game.Sync;

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