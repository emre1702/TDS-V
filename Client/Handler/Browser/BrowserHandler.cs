using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Browser
{
    public class BrowserHandler : ServiceBase
    {
        public BrowserHandler(LoggingHandler loggingHandler, EventsHandler eventsHandler,
            RemoteEventsSender remoteEventsSender)
            : base(loggingHandler)
        {
            Angular = new AngularBrowserHandler(loggingHandler, eventsHandler);
            RegisterLogin = new RegisterLoginBrowserHandler(loggingHandler);
            MapCreatorObjectChoice = new MapCreatorObjectChoiceBrowserHandler(loggingHandler);
            MapCreatorVehicleChoice = new MapCreatorVehicleChoiceBrowserHandler(loggingHandler);
            PlainMain = new PlainMainBrowserHandler(loggingHandler, remoteEventsSender, eventsHandler);

            eventsHandler.LanguageChanged += EventsHandler_LanguageChanged;

            RAGE.Events.Add(FromBrowserEvent.InputStarted, _ => InInput = true);
            RAGE.Events.Add(FromBrowserEvent.InputStopped, _ => InInput = false);
            RAGE.Events.Add(ToClientEvent.SendAlert, SendAlert);
        }

        public AngularBrowserHandler Angular { get; }
        public bool InInput { get; private set; }
        public MapCreatorObjectChoiceBrowserHandler MapCreatorObjectChoice { get; }
        public MapCreatorVehicleChoiceBrowserHandler MapCreatorVehicleChoice { get; }
        public PlainMainBrowserHandler PlainMain { get; }
        public RegisterLoginBrowserHandler RegisterLogin { get; }

        private void EventsHandler_LanguageChanged(ILanguage lang, bool beforeLogin)
        {
            if (!(Angular.Browser is null))
                Angular.LoadLanguage(lang);
            if (!(RegisterLogin.Browser is null))
                RegisterLogin.SyncLanguage(lang);
        }

        private void SendAlert(object[] args)
        {
            var msg = (string)args[0];
            RAGE.Ui.DefaultWindow.ExecuteJs($"alert(`{msg.Replace("`", "\"")}`)");
        }
    }
}
