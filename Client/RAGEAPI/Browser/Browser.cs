using RAGE.Ui;
using TDS_Client.Data.Interfaces.ModAPI.Browser;

namespace TDS_Client.RAGEAPI.Browser
{
    class Browser : IBrowser
    {
        private readonly HtmlWindow _instance;

        internal Browser(HtmlWindow instance) 
            => _instance = instance;

        public void Destroy()
        {
            _instance.Destroy();
        }

        public void ExecuteJs(string js)
        {
            _instance.ExecuteJs(js);
        }
    }
}
