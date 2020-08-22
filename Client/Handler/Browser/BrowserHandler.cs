using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;

namespace TDS_Client.Handler.Browser
{
    public class BrowserHandler : ServiceBase
    {
        #region Public Constructors

        public BrowserHandler(IModAPI modAPI, LoggingHandler loggingHandler, EventsHandler eventsHandler,
            Serializer serializer, RemoteEventsSender remoteEventsSender)
            : base(modAPI, loggingHandler)
        {
            Angular = new AngularBrowserHandler(modAPI, loggingHandler, serializer, eventsHandler);
            RegisterLogin = new RegisterLoginBrowserHandler(modAPI, loggingHandler, serializer);
            MapCreatorObjectChoice = new MapCreatorObjectChoiceBrowserHandler(modAPI, loggingHandler, serializer);
            MapCreatorVehicleChoice = new MapCreatorVehicleChoiceBrowserHandler(modAPI, loggingHandler, serializer);
            PlainMain = new PlainMainBrowserHandler(modAPI, loggingHandler, serializer, remoteEventsSender, eventsHandler);

            eventsHandler.LanguageChanged += EventsHandler_LanguageChanged;

            modAPI.Event.Add(FromBrowserEvent.InputStarted, _ => InInput = true);
            modAPI.Event.Add(FromBrowserEvent.InputStopped, _ => InInput = false);
        }

        #endregion Public Constructors

        #region Public Properties

        public AngularBrowserHandler Angular { get; }
        public bool InInput { get; private set; }
        public MapCreatorObjectChoiceBrowserHandler MapCreatorObjectChoice { get; }
        public MapCreatorVehicleChoiceBrowserHandler MapCreatorVehicleChoice { get; }
        public PlainMainBrowserHandler PlainMain { get; }
        public RegisterLoginBrowserHandler RegisterLogin { get; }

        #endregion Public Properties

        #region Private Methods

        private void EventsHandler_LanguageChanged(ILanguage lang, bool beforeLogin)
        {
            if (!(RegisterLogin.Browser is null))
                RegisterLogin.SyncLanguage(lang);
        }

        #endregion Private Methods
    }
}
