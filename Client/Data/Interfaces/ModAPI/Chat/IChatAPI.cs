namespace TDS_Client.Data.Interfaces.ModAPI.Chat
{
    public interface IChatAPI
    {
        #region Public Properties

        bool SafeMode { get; set; }

        #endregion Public Properties

        #region Public Methods

        void Output(string msg);

        void Show(bool show);

        #endregion Public Methods
    }
}
