using TDS_Client.Data;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler;
using TDS_Client.Handler.Account;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Events;
using TDS_Client.Manager.Utility;
using TDS_Shared.Core;

namespace TDS_Client.Core.Init
{
    class Services
    {
        internal static void Initialize(IModAPI modAPI)
        {
            var serializer = new Serializer();

            var eventsHandler = new EventsHandler();

            var registerLoginHandler = new RegisterLoginHandler(modAPI);
            var cursorHandler = new CursorHandler(modAPI, eventsHandler);
            new SettingsHandler(modAPI);
            new EventsHandler();
            new RemoteEventsSender(modAPI);
            var dxHandler = new DxHandler(modAPI);
            new TimerHandler(modAPI, dxHandler);
            new CamerasHandler(modAPI);
            new UtilsHandler(modAPI, serializer);
        }
    }
}
