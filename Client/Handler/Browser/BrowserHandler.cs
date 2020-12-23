using TDS.Client.Data.Defaults;
using TDS.Client.Data.Interfaces;
using TDS.Client.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Default;

namespace TDS.Client.Handler.Browser
{
    public class BrowserHandler : ServiceBase
    {
        private TDSTimer _browserCreatedCheckTimer;

        public BrowserHandler(LoggingHandler loggingHandler, EventsHandler eventsHandler,
            RemoteEventsSender remoteEventsSender)
            : base(loggingHandler)
        {
            Angular = new AngularBrowserHandler(loggingHandler, eventsHandler);
            MapCreatorObjectChoice = new MapCreatorObjectChoiceBrowserHandler(loggingHandler);
            MapCreatorVehicleChoice = new MapCreatorVehicleChoiceBrowserHandler(loggingHandler);
            PlainMain = new PlainMainBrowserHandler(loggingHandler, remoteEventsSender, eventsHandler);

            eventsHandler.LanguageChanged += EventsHandler_LanguageChanged;

            RAGE.Events.Add(FromBrowserEvent.InputStarted, _ => InInput = true);
            RAGE.Events.Add(FromBrowserEvent.InputStopped, _ => InInput = false);
            RAGE.Events.Add(ToClientEvent.SendAlert, SendAlert);
            RAGE.Events.Add(FromBrowserEvent.Created, BrowserSentCreated);

            _browserCreatedCheckTimer = new TDSTimer(MakeSureBrowsersAreCreatedCorrectly, 10000);
        }

        public AngularBrowserHandler Angular { get; }
        public bool InInput { get; private set; }
        public MapCreatorObjectChoiceBrowserHandler MapCreatorObjectChoice { get; }
        public MapCreatorVehicleChoiceBrowserHandler MapCreatorVehicleChoice { get; }
        public PlainMainBrowserHandler PlainMain { get; }

        private void EventsHandler_LanguageChanged(ILanguage lang, bool beforeLogin)
        {
            if (!(Angular.Browser is null))
                Angular.LoadLanguage(lang);
        }

        private void SendAlert(object[] args)
        {
            var msg = (string)args[0];
            RAGE.Ui.DefaultWindow.ExecuteJs($"alert(`{msg.Replace("`", "\"")}`)");
        }

        private void MakeSureBrowsersAreCreatedCorrectly()
        {
            bool oneNotCreated = false;
            if (!(Angular.Browser is null) && !Angular.CreatedSuccessfully)
            {
                Angular.Browser.Destroy();
                Angular.CreateBrowser();
                oneNotCreated = true;
            }

            if (!(PlainMain.Browser is null) && !PlainMain.CreatedSuccessfully)
            {
                PlainMain.Browser.Destroy();
                PlainMain.CreateBrowser();
                oneNotCreated = true;
            }

            if (!oneNotCreated && Angular.HasBeenCreatedOnce && PlainMain.HasBeenCreatedOnce)
            {
                _browserCreatedCheckTimer?.Kill();
                _browserCreatedCheckTimer = null;
            }
        }

        private void BrowserSentCreated(object[] args)
        {
            var browserName = (string)args[0];
            switch (browserName)
            {
                case "Angular":
                    Angular.CreatedSuccessfully = true;
                    RAGE.Ui.Console.LogLine(RAGE.Ui.ConsoleVerbosity.Info, "Angular browser has been created successfully.", true);
                    Angular.Browser.MarkAsChat();
                    RAGE.Chat.Show(true);
                    Angular.ProcessExecuteList();
                    break;

                case "PlainMain":
                    PlainMain.CreatedSuccessfully = true;
                    PlainMain.ProcessExecuteList();
                    break;
            }
        }
    }
}
