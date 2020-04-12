using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;

namespace TDS_Client.Handler.Browser
{
    public class BrowserHandler
    {
        public bool InInput { get; private set; }

        public AngularBrowserHandler Angular { get; }
        public RegisterLoginBrowserHandler RegisterLogin { get; }
        public MapCreatorObjectChoiceBrowserHandler MapCreatorObjectChoice { get; }
        public MapCreatorVehicleChoiceBrowserHandler MapCreatorVehicleChoice { get; }
        public PlainMainBrowserHandler PlainMain { get; }

        public BrowserHandler(IModAPI modAPI, SettingsHandler settingsHandler, CursorHandler cursorHandler, EventsHandler eventsHandler, Serializer serializer)
        {
            Angular = new AngularBrowserHandler(modAPI, settingsHandler, cursorHandler, serializer, eventsHandler);
            RegisterLogin = new RegisterLoginBrowserHandler(modAPI, serializer);
            MapCreatorObjectChoice = new MapCreatorObjectChoiceBrowserHandler(modAPI, serializer);
            MapCreatorVehicleChoice = new MapCreatorVehicleChoiceBrowserHandler(modAPI, serializer);

            eventsHandler.LanguageChanged += EventsHandler_LanguageChanged;

            modAPI.Event.Add(FromBrowserEvent.InputStarted, _ => InInput = true);
            modAPI.Event.Add(FromBrowserEvent.InputStopped, _ => InInput = false);
        }

        private void EventsHandler_LanguageChanged(ILanguage lang, bool beforeLogin)
        {
            if (!(Angular.Browser is null))
                Angular.LoadLanguage(lang);
            if (!(RegisterLogin.Browser is null))
                RegisterLogin.SyncLanguage(lang);
        }
    }
}
