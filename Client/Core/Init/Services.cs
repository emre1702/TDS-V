using TDS_Client.Data;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler;
using TDS_Client.Handler.Account;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Sync;
using TDS_Client.Manager.Browser;
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
            var bindsHandler = new BindsHandler(modAPI);
            var discordHandler = new DiscordHandler(modAPI);

            var cursorHandler = new CursorHandler(modAPI, eventsHandler, bindsHandler);
            
            var dxHandler = new DxHandler(modAPI);
            var timerHandler = new TimerHandler(modAPI, dxHandler);
            var remoteEventsSender = new RemoteEventsSender(modAPI, timerHandler);
            var settingsHandler = new SettingsHandler(modAPI, remoteEventsSender, eventsHandler);
            var camerasHandler = new CamerasHandler(modAPI);
            var browserHandler = new BrowserHandler(modAPI, settingsHandler, cursorHandler, eventsHandler, serializer);
            var dataSyncHandler = new DataSyncHandler(eventsHandler, modAPI, browserHandler, lobbyHandler, serializer);
            var utilsHandler = new UtilsHandler(modAPI, serializer, dataSyncHandler);
            var registerLoginHandler = new RegisterLoginHandler(cursorHandler, remoteEventsSender, browserHandler);
            var voiceHandler = new VoiceHandler(bindsHandler, settingsHandler, browserHandler, modAPI, utilsHandler, eventsHandler);



            var chatHandler = new ChatHandler(browserHandler, modAPI, bindsHandler);
        }
    }
}
