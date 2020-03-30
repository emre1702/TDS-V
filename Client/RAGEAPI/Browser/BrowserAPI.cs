using RAGE.Ui;
using TDS_Client.Data.Interfaces.ModAPI.Browser;

namespace TDS_Client.RAGEAPI.Browser
{
    class BrowserAPI : IBrowserAPI
    {
        public IBrowser Create(string url)
        {
            var modBrowser = new HtmlWindow(url);
            return new Browser(modBrowser);
        }
    }
}
