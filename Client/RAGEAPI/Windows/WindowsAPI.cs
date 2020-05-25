using TDS_Client.Data.Interfaces.ModAPI.Windows;

namespace TDS_Client.RAGEAPI.Windows
{
    internal class WindowsAPI : IWindowsAPI
    {
        #region Public Properties

        public bool Focused => RAGE.Ui.Windows.Focused;

        public bool Fullscreen => RAGE.Ui.Windows.Fullscreen;

        #endregion Public Properties

        #region Public Methods

        public void Notify(string title, string text = "", string attribute = "", int duration = 0, bool silent = false)
        {
            if (!Focused)
                RAGE.Ui.Windows.Notify(title, text, attribute, duration, silent);
        }

        #endregion Public Methods
    }
}
