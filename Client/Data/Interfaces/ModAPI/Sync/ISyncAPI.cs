namespace TDS_Client.Data.Interfaces.ModAPI.Sync
{
    public interface ISyncAPI
    {
        #region Public Methods

        void SendEvent(string eventName, params object[] args);

        #endregion Public Methods
    }
}
