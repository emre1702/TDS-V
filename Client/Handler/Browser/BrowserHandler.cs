using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;

namespace TDS_Client.Handler.Browser
{
    public class BrowserHandler : ServiceBase
    {
        public BrowserHandler(LoggingHandler loggingHandler, EventsHandler eventsHandler,
            Serializer serializer, RemoteEventsSender remoteEventsSender)
            : base(loggingHandler)
        {
            Angular = new AngularBrowserHandler(loggingHandler, serializer, eventsHandler);
            RegisterLogin = new RegisterLoginBrowserHandler(loggingHandler, serializer);
            MapCreatorObjectChoice = new MapCreatorObjectChoiceBrowserHandler(loggingHandler, serializer);
            MapCreatorVehicleChoice = new MapCreatorVehicleChoiceBrowserHandler(loggingHandler, serializer);
            PlainMain = new PlainMainBrowserHandler(loggingHandler, serializer, remoteEventsSender, eventsHandler);

            eventsHandler.LanguageChanged += EventsHandler_LanguageChanged;

            RAGE.Events.Add(FromBrowserEvent.InputStarted, _ => InInput = true);
            RAGE.Events.Add(FromBrowserEvent.InputStopped, _ => InInput = false);
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
    }
}
