namespace TDS_Client.Data.Interfaces.ModAPI.Browser
{
    public interface IBrowser
    {
        #region Public Methods

        void Call(string eventName, params object[] args);

        void Destroy();

        void ExecuteJs(string js);

        void MarkAsChat();

        #endregion Public Methods
    }
}
