using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Data.Interfaces;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Events;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Browser.Angular;
using TDS_Client.Manager.Utility;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;

namespace TDS_Client.Handler.Browser
{
    public class BrowserHandler
    {
        public bool InInput { get; set; }

        public AngularBrowserHandler Angular { get; }
        public RegisterLoginBrowserHandler RegisterLogin { get; }
        public MapCreatorObjectChoiceBrowserHandler MapCreatorObjectChoice { get; }
        public MapCreatorVehicleChoiceBrowserHandler MapCreatorVehicleChoice { get; }
        public PlainMainBrowserHandler PlainMain { get; }

        public BrowserHandler(IModAPI modAPI, SettingsHandler settingsHandler, CursorHandler cursorHandler, EventsHandler eventsHandler, Serializer serializer)
        {
            Angular = new AngularBrowserHandler(modAPI, settingsHandler, cursorHandler, serializer);
            RegisterLogin = new RegisterLoginBrowserHandler(modAPI, serializer);
            MapCreatorObjectChoice = new MapCreatorObjectChoiceBrowserHandler(modAPI, serializer);
            MapCreatorVehicleChoice = new MapCreatorVehicleChoiceBrowserHandler(modAPI, serializer);

            eventsHandler.LanguageChanged += EventsHandler_LanguageChanged;
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
