using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Deathmatch;
using TDS_Client.Handler.Draw;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Lobby;
using TDS_Client.Handler.Map;
using TDS_Client.Handler.MapCreator;
using TDS_Client.Handler.Sync;
using TDS_Client.Manager.Utility;
using TDS_Shared.Core;

namespace TDS_Client.Core.Init
{
    class Services
    {
        internal static void Initialize(IModAPI modAPI)
        {
            var serializer = new Serializer();

            var dxHandler = new DxHandler(modAPI);
            var timerHandler = new TimerHandler(modAPI, dxHandler);
            var remoteEventsSender = new RemoteEventsSender(modAPI, timerHandler);

            var eventsHandler = new EventsHandler(modAPI, remoteEventsSender);
            var bindsHandler = new BindsHandler(modAPI);
            var discordHandler = new DiscordHandler(modAPI, eventsHandler);

            var cursorHandler = new CursorHandler(modAPI, eventsHandler, bindsHandler);
            var settingsHandler = new SettingsHandler(modAPI, remoteEventsSender, eventsHandler);
            
            var browserHandler = new BrowserHandler(modAPI, settingsHandler, cursorHandler, eventsHandler, serializer, remoteEventsSender);
            var dataSyncHandler = new DataSyncHandler(eventsHandler, modAPI, browserHandler, serializer);
            var utilsHandler = new UtilsHandler(modAPI, serializer, dataSyncHandler, eventsHandler);


            new SuicideAnimHandler(modAPI, remoteEventsSender, utilsHandler);

            var soundsHandler = new SoundsHandler(modAPI, settingsHandler);

            var scaleformMessageHandler = new ScaleformMessageHandler(modAPI, settingsHandler, timerHandler);
            var deathHandler = new DeathHandler(modAPI, settingsHandler, scaleformMessageHandler, eventsHandler, utilsHandler, browserHandler);
            var camerasHandler = new CamerasHandler(modAPI, utilsHandler, remoteEventsSender, bindsHandler, deathHandler, eventsHandler);
            new UserpanelHandler(modAPI, browserHandler, cursorHandler, settingsHandler, remoteEventsSender, serializer, eventsHandler, bindsHandler);

            var registerLoginHandler = new RegisterLoginHandler(modAPI, cursorHandler, remoteEventsSender, browserHandler, settingsHandler, serializer, eventsHandler);
            var voiceHandler = new VoiceHandler(bindsHandler, settingsHandler, browserHandler, modAPI, utilsHandler, eventsHandler);
            var forceStayAtPosHandler = new ForceStayAtPosHandler(modAPI, remoteEventsSender, settingsHandler, dxHandler, timerHandler, serializer);
            new CrouchingHandler(modAPI, eventsHandler, dataSyncHandler, remoteEventsSender);
            var instructionalButtonHandler = new InstructionalButtonHandler(modAPI, eventsHandler, settingsHandler);
            new MidsizedMessageHandler(modAPI, timerHandler);

            var floatingDamageInfoHandler = new FloatingDamageInfoHandler(modAPI, timerHandler, settingsHandler, eventsHandler, dxHandler);
            var playerFightHandler = new PlayerFightHandler(modAPI, eventsHandler, settingsHandler, browserHandler, floatingDamageInfoHandler, utilsHandler, camerasHandler);
            new AntiCheatHandler(modAPI, playerFightHandler);
            var mapLimitHandler = new MapLimitHandler(settingsHandler, modAPI, remoteEventsSender, eventsHandler, dxHandler, timerHandler);

            var lobbyHandler = new LobbyHandler(modAPI, browserHandler, playerFightHandler, instructionalButtonHandler, eventsHandler, settingsHandler, bindsHandler, remoteEventsSender, dxHandler,
                timerHandler, utilsHandler, camerasHandler, cursorHandler, dataSyncHandler, mapLimitHandler, serializer);
            var scoreboardHandler = new ScoreboardHandler(dxHandler, modAPI, settingsHandler, lobbyHandler, timerHandler, remoteEventsSender, eventsHandler, bindsHandler, serializer);
            var chatHandler = new ChatHandler(browserHandler, modAPI, bindsHandler, remoteEventsSender, lobbyHandler, playerFightHandler);


            var workaroundsHandler = new WorkaroundsHandler(serializer, modAPI, utilsHandler);
            new AFKCheckHandler(eventsHandler, modAPI, settingsHandler, remoteEventsSender, playerFightHandler, timerHandler, dxHandler);

            var nametagsHandler = new NametagsHandler(modAPI, camerasHandler, settingsHandler, utilsHandler);

            new RankingHandler(modAPI, camerasHandler, utilsHandler, settingsHandler, cursorHandler, browserHandler, nametagsHandler, deathHandler, eventsHandler);
            new MapCreatorHandler(modAPI, bindsHandler, instructionalButtonHandler, settingsHandler, utilsHandler, camerasHandler, cursorHandler, browserHandler, dxHandler,
                remoteEventsSender, serializer, eventsHandler, lobbyHandler, timerHandler);


        }
    }
}
