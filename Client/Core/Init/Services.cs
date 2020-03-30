using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Account;
using TDS_Client.Handler.Events;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Core.Init
{
    class Services
    {
        internal static void Initialize(IModAPI modAPI)
        {
            var eventsHandler = new EventsHandler();

            var registerLoginHandler = new RegisterLoginHandler(modAPI);
            var cursorHandler = new CursorHandler(modAPI, eventsHandler);
        }
    }
}
