using RAGE.Ui;

namespace TDS_Client.Instance.Draw.Browser
{
    class Angular
    {
        private HtmlWindow browser;

        public Angular(string path, bool isChat = false)
        {
            browser = new HtmlWindow(path);
            if (isChat)
                browser.MarkAsChat();
        }

        void Destroy()
        {
            browser.Destroy();
            browser = null;
        }

        void Execute(string str)
        {
            if (browser != null)
                browser.ExecuteJs(str);
        }
    }
}
