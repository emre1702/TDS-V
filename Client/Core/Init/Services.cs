using System;
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
            var loggingHandler = new LoggingHandler(modAPI);
            var serializer = new Serializer(str => loggingHandler.LogInfo(str, "Serializer"), ex => loggingHandler.LogError(ex));

            try
            {
                loggingHandler.LogInfo("Initializing services ...", "Services.Initialize");

                var dxHandler = new DxHandler(modAPI, loggingHandler);
                var timerHandler = new TimerHandler(modAPI, loggingHandler, dxHandler);
                var remoteEventsSender = new RemoteEventsSender(modAPI, loggingHandler, timerHandler);

                var eventsHandler = new EventsHandler(modAPI, loggingHandler, remoteEventsSender);
                var bindsHandler = new BindsHandler(modAPI, loggingHandler);
                var discordHandler = new DiscordHandler(modAPI, loggingHandler, eventsHandler);

                var browserHandler = new BrowserHandler(modAPI, loggingHandler, eventsHandler, serializer, remoteEventsSender);
                var settingsHandler = new SettingsHandler(modAPI, loggingHandler, remoteEventsSender, eventsHandler, browserHandler, serializer);
                var cursorHandler = new CursorHandler(modAPI, loggingHandler, eventsHandler, bindsHandler, settingsHandler);
                
                var dataSyncHandler = new DataSyncHandler(modAPI, loggingHandler, eventsHandler, browserHandler, serializer);
                var utilsHandler = new UtilsHandler(modAPI, loggingHandler, serializer, dataSyncHandler, eventsHandler);
                new GangHousesHandler(modAPI, loggingHandler, eventsHandler, settingsHandler, serializer);
                
                new SuicideAnimHandler(modAPI, loggingHandler, remoteEventsSender, utilsHandler);

                var soundsHandler = new SoundsHandler(modAPI, loggingHandler, settingsHandler);

                var scaleformMessageHandler = new ScaleformMessageHandler(modAPI, loggingHandler, settingsHandler, timerHandler);
                var deathHandler = new DeathHandler(modAPI, loggingHandler, settingsHandler, scaleformMessageHandler, eventsHandler, utilsHandler, browserHandler);
                var camerasHandler = new CamerasHandler(modAPI, loggingHandler, utilsHandler, remoteEventsSender, bindsHandler, deathHandler, eventsHandler);
                new UserpanelHandler(modAPI, loggingHandler, browserHandler, cursorHandler, settingsHandler, remoteEventsSender, serializer, eventsHandler, bindsHandler);

                var registerLoginHandler = new RegisterLoginHandler(modAPI, loggingHandler, cursorHandler, remoteEventsSender, browserHandler, settingsHandler, serializer, eventsHandler);
                var voiceHandler = new VoiceHandler(modAPI, loggingHandler, bindsHandler, settingsHandler, browserHandler, utilsHandler, eventsHandler);
                var forceStayAtPosHandler = new ForceStayAtPosHandler(modAPI, loggingHandler, remoteEventsSender, settingsHandler, dxHandler, timerHandler, serializer);
                new CrouchingHandler(modAPI, loggingHandler, eventsHandler, dataSyncHandler, remoteEventsSender);
                var instructionalButtonHandler = new InstructionalButtonHandler(modAPI, loggingHandler, eventsHandler, settingsHandler);
                new MidsizedMessageHandler(modAPI, loggingHandler, timerHandler);

                var floatingDamageInfoHandler = new FloatingDamageInfoHandler(modAPI, loggingHandler, timerHandler, settingsHandler, eventsHandler, dxHandler);
                var playerFightHandler = new PlayerFightHandler(modAPI, loggingHandler, eventsHandler, settingsHandler, browserHandler, floatingDamageInfoHandler, utilsHandler, camerasHandler);
                new AntiCheatHandler(modAPI, loggingHandler, playerFightHandler);
                var mapLimitHandler = new MapLimitHandler(modAPI, loggingHandler, settingsHandler, remoteEventsSender, eventsHandler, dxHandler, timerHandler);
               

                var lobbyHandler = new LobbyHandler(modAPI, loggingHandler, browserHandler, playerFightHandler, instructionalButtonHandler, eventsHandler, settingsHandler, bindsHandler, remoteEventsSender, dxHandler,
                    timerHandler, utilsHandler, camerasHandler, cursorHandler, dataSyncHandler, mapLimitHandler, serializer);
                new DamageHandler(modAPI, browserHandler, remoteEventsSender, playerFightHandler, lobbyHandler);
                var scoreboardHandler = new ScoreboardHandler(modAPI, loggingHandler, dxHandler, settingsHandler, lobbyHandler, timerHandler, remoteEventsSender, eventsHandler, bindsHandler, serializer);
                var chatHandler = new ChatHandler(modAPI, loggingHandler, browserHandler, bindsHandler, remoteEventsSender, lobbyHandler, playerFightHandler, eventsHandler);


                var workaroundsHandler = new WorkaroundsHandler(modAPI, loggingHandler, serializer, utilsHandler);
                new AFKCheckHandler(modAPI, loggingHandler, eventsHandler, settingsHandler, remoteEventsSender, playerFightHandler, timerHandler, dxHandler);

                var nametagsHandler = new NametagsHandler(modAPI, loggingHandler, camerasHandler, settingsHandler, utilsHandler);

                new RankingHandler(modAPI, loggingHandler, camerasHandler, utilsHandler, settingsHandler, cursorHandler, browserHandler, nametagsHandler, deathHandler, eventsHandler);
                new MapCreatorHandler(modAPI, loggingHandler, bindsHandler, instructionalButtonHandler, settingsHandler, utilsHandler, camerasHandler, cursorHandler, browserHandler, dxHandler,
                    remoteEventsSender, serializer, eventsHandler, lobbyHandler, timerHandler, dataSyncHandler);

                loggingHandler.LogInfo("Services successfully initialized", "Services.Initialize", true);
            }
            catch (Exception ex)
            {
                loggingHandler.LogError(ex);
            }

        }
    }
}
