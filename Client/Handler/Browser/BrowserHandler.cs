﻿using TDS.Client.Data.Defaults;
using TDS.Client.Data.Interfaces;
using TDS.Client.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Default;

namespace TDS.Client.Handler.Browser
{
    public class BrowserHandler : ServiceBase
    {
        private readonly EventsHandler _eventsHandler;
        private TDSTimer _browserCreatedCheckTimer;

        public BrowserHandler(LoggingHandler loggingHandler, EventsHandler eventsHandler,
            RemoteEventsSender remoteEventsSender)
            : base(loggingHandler)
        {
            _eventsHandler = eventsHandler;
            Angular = new AngularBrowserHandler(loggingHandler, eventsHandler);
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
                    Logging.LogInfo("Angular browser has been created successfully.", "BrowserHandler");
                    Angular.Browser.MarkAsChat();
                    Angular.ProcessExecuteList();
                    _eventsHandler.OnAngularBrowserCreated();
                    break;

                case "PlainMain":
                    PlainMain.CreatedSuccessfully = true;
                    PlainMain.ProcessExecuteList();
                    break;
            }
        }
    }
}