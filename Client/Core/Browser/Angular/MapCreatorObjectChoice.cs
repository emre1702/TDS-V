using RAGE.Ui;
using System.Collections.Generic;
using TDS_Client.Default;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Manager.Browser.Angular
{
    static class MapCreatorObjectChoice
    {
        public static HtmlWindow Browser { get; set; }
        private readonly static Queue<string> _executeQueue = new Queue<string>();

        private static void Execute(string eventName, params object[] args)
        {
            string execStr = Shared.GetExecStr(eventName, args);
            if (Browser == null)
                _executeQueue.Enqueue(execStr);
            else
                Browser.ExecuteJs(execStr);
        }

        public static void Start()
        {
            Browser = new HtmlWindow(Constants.AngularMapCreatorObjectChoiceBrowserPath);

            Execute(DToBrowserEvent.InitLoadAngular, (int)Settings.LanguageEnum);
            foreach (var execStr in _executeQueue)
            {
                Browser.ExecuteJs(execStr);
            }
            _executeQueue.Clear();
        }

        public static void Stop()
        {
            if (Browser == null)
                return;
            Browser.Destroy();
            Browser = null;
            _executeQueue.Clear();
        }


    }
}
