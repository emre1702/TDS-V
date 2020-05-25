using RAGE.Ui;
using TDS_Client.Data.Interfaces.ModAPI.Browser;

namespace TDS_Client.RAGEAPI.Browser
{
    internal class BrowserAPI : IBrowserAPI
    {
        #region Public Methods

        public IBrowser Create(string url)
        {
            var modBrowser = new HtmlWindow(url);
            return new Browser(modBrowser);
        }

        #endregion Public Methods
    }
}
