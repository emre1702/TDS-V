using RAGE.Ui;
using TDS_Client.Data.Interfaces.ModAPI.Browser;

namespace TDS_Client.RAGEAPI.Browser
{
    class Browser : IBrowser
    {
        private readonly HtmlWindow _instance;

        internal Browser(HtmlWindow instance) 
            => _instance = instance;

        public void Call(string eventName, params object[] args)
        {
            _instance.Call(eventName, args);
        }

        public void Destroy()
        {
            _instance.Destroy();
        }

        public void ExecuteJs(string js)
        {
            _instance.ExecuteJs(js);
        }

        public void MarkAsChat()
        {
            _instance.MarkAsChat();
        }
    }
}
