using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler;
using TDS_Client.Handler.Account;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Deathmatch;
using TDS_Client.Handler.Draw;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.MapCreator;
using TDS_Client.Handler.Sync;
using TDS_Shared.Core;

namespace TDS_Client.Core.Init
{
    class Services
    {
        internal static void Initialize(IModAPI modAPI)
        {
            var serializer = new Serializer();

            var eventsHandler = new EventsHandler(modAPI);
            var bindsHandler = new BindsHandler(modAPI);
            var discordHandler = new DiscordHandler(modAPI);


            var cursorHandler = new CursorHandler(modAPI, eventsHandler, bindsHandler);
            var spectatingHandler = new SpectatingHandler();

            var dxHandler = new DxHandler(modAPI);
            var timerHandler = new TimerHandler(modAPI, dxHandler);
            var remoteEventsSender = new RemoteEventsSender(modAPI, timerHandler);
            new SuicideAnimHandler(modAPI, remoteEventsSender);
            var settingsHandler = new SettingsHandler(modAPI, remoteEventsSender, eventsHandler);
            var browserHandler = new BrowserHandler(modAPI, settingsHandler, cursorHandler, eventsHandler, serializer);
            var dataSyncHandler = new DataSyncHandler(eventsHandler, modAPI, browserHandler, lobbyHandler, serializer);
            var utilsHandler = new UtilsHandler(modAPI, serializer, dataSyncHandler);
            var camerasHandler = new CamerasHandler(modAPI, utilsHandler, spectatingHandler);
            new UserpanelHandler(browserHandler, cursorHandler, settingsHandler);
            var scaleformMessageHandler = new ScaleformMessageHandler(modAPI, settingsHandler, timerHandler);


            var registerLoginHandler = new RegisterLoginHandler(cursorHandler, remoteEventsSender, browserHandler, settingsHandler);
            var voiceHandler = new VoiceHandler(bindsHandler, settingsHandler, browserHandler, modAPI, utilsHandler, eventsHandler);
            var forceStayAtPosHandler = new ForceStayAtPosHandler(modAPI, remoteEventsSender, settingsHandler);
            new CrouchingHandler(eventsHandler, dataSyncHandler, remoteEventsSender);
            var instructionalButtonHandler = new InstructionalButtonHandler(modAPI, eventsHandler, settingsHandler);
            new MidsizedMessageHandler(modAPI, timerHandler);
            new ScoreboardHandler(dxHandler, modAPI, settingsHandler, teamsHandler, timerHandler, remoteEventsSender, eventsHandler);
            var chatHandler = new ChatHandler(browserHandler, modAPI, bindsHandler);

            var floatingDamageInfoHandler = new FloatingDamageInfoHandler(modAPI, timerHandler, settingsHandler, eventsHandler);
            var playerFightHandler = new PlayerFightHandler(modAPI, eventsHandler, settingsHandler, browserHandler, floatingDamageInfoHandler);
            new AntiCheatHandler(modAPI, playerFightHandler);
            var workaroundsHandler = new WorkaroundsHandler(serializer, modAPI, utilsHandler);
            new AFKCheckHandler(eventsHandler, modAPI, settingsHandler, remoteEventsSender, playerFightHandler);

            var nametagsHandler = new NametagsHandler(modAPI, camerasHandler, settingsHandler, utilsHandler);
            var deathHandler = new DeathHandler(modAPI, settingsHandler, scaleformMessageHandler);
            new RankingHandler(modAPI, camerasHandler, utilsHandler, settingsHandler, cursorHandler, browserHandler, nametagsHandler, deathHandler);
            new MapCreatorHandler(modAPI, bindsHandler, instructionalButtonHandler, settingsHandler, utilsHandler, camerasHandler, spectatingHandler, cursorHandler, browserHandler, dxHandler,
                remoteEventsSender, serializer);


        }
    }
}
