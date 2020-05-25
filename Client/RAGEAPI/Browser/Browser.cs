using RAGE.Ui;
using TDS_Client.Data.Interfaces.ModAPI.Browser;

namespace TDS_Client.RAGEAPI.Browser
{
    internal class Browser : IBrowser
    {
        #region Private Fields

        private readonly HtmlWindow _instance;

        #endregion Private Fields

        #region Internal Constructors

        internal Browser(HtmlWindow instance)
            => _instance = instance;

        #endregion Internal Constructors

        #region Public Methods

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

        #endregion Public Methods
    }
}
