namespace TDS_Client.Data.Interfaces.ModAPI.Windows
{
    public interface IWindowsAPI
    {
        #region Public Properties

        bool Focused { get; }
        bool Fullscreen { get; }

        #endregion Public Properties

        #region Public Methods

        void Notify(string title, string text = "", string attribute = "", int duration = 0, bool silent = false);

        #endregion Public Methods
    }
}
