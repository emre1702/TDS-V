using System;
using TDS_Client.Handler;
using TDS_Client.Handler.Appearance;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Deathmatch;
using TDS_Client.Handler.Draw;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Factories;
using TDS_Client.Handler.GangSystem;
using TDS_Client.Handler.Lobby;
using TDS_Client.Handler.Map;
using TDS_Client.Handler.MapCreator;
using TDS_Client.Handler.Sync;
using TDS_Client.Manager.Utility;
using TDS_Shared.Core;

namespace TDS_Client.Core.Init
{
    internal class Services
    {
        internal static void Initialize()
        {
            var loggingHandler = new LoggingHandler();
            Serializer.Init(str => loggingHandler.LogInfo(str, "Serializer"), ex => loggingHandler.LogError(ex));

            try
            {
                loggingHandler.LogInfo("Initializing services ...", "Services.Initialize");
                var timerHandler = new TimerHandler(loggingHandler);

                new PlayerFactory();

                var dxHandler = new DxHandler(loggingHandler);
                var remoteEventsSender = new RemoteEventsSender(loggingHandler, timerHandler);

                var eventsHandler = new EventsHandler(loggingHandler, remoteEventsSender);
                var bindsHandler = new BindsHandler(loggingHandler);
                var discordHandler = new DiscordHandler(loggingHandler, eventsHandler);

                var browserHandler = new BrowserHandler(loggingHandler, eventsHandler, remoteEventsSender);
                var settingsHandler = new SettingsHandler(loggingHandler, remoteEventsSender, eventsHandler, browserHandler);
                var cursorHandler = new CursorHandler(loggingHandler, eventsHandler, bindsHandler, settingsHandler);
                new FreeroamHandler(loggingHandler, eventsHandler, browserHandler);

                var dataSyncHandler = new DataSyncHandler(loggingHandler, eventsHandler, browserHandler);
                var utilsHandler = new UtilsHandler(loggingHandler, dataSyncHandler, eventsHandler);
                new GangHousesHandler(loggingHandler, eventsHandler, settingsHandler);
                new GangVehiclesHandler(loggingHandler, dataSyncHandler, eventsHandler);
                new GhostModeHandler(loggingHandler, eventsHandler);

                new SuicideAnimHandler(loggingHandler, remoteEventsSender, utilsHandler);

                var soundsHandler = new SoundsHandler(loggingHandler, settingsHandler);

                var scaleformMessageHandler = new ScaleformMessageHandler(loggingHandler, settingsHandler, timerHandler);
                var deathHandler = new DeathHandler(loggingHandler, settingsHandler, scaleformMessageHandler, eventsHandler, utilsHandler, browserHandler);
                var camerasHandler = new CamerasHandler(loggingHandler, utilsHandler, remoteEventsSender, bindsHandler, deathHandler, eventsHandler);
                var instructionalButtonHandler = new InstructionalButtonHandler(loggingHandler, eventsHandler, settingsHandler);
                new UserpanelHandler(loggingHandler, browserHandler, cursorHandler, settingsHandler, remoteEventsSender, eventsHandler, bindsHandler, instructionalButtonHandler);
                new CharCreatorHandler(loggingHandler, browserHandler, deathHandler, camerasHandler, eventsHandler, cursorHandler, utilsHandler);

                var registerLoginHandler = new RegisterLoginHandler(loggingHandler, cursorHandler, remoteEventsSender, browserHandler, settingsHandler, eventsHandler);
                var voiceHandler = new VoiceHandler(loggingHandler, bindsHandler, browserHandler, utilsHandler, eventsHandler);
                var forceStayAtPosHandler = new ForceStayAtPosHandler(loggingHandler, remoteEventsSender, settingsHandler, dxHandler, timerHandler);
                new CrouchingHandler(loggingHandler, eventsHandler, dataSyncHandler, remoteEventsSender);

                new MidsizedMessageHandler(loggingHandler, timerHandler);

                var floatingDamageInfoHandler = new FloatingDamageInfoHandler(loggingHandler, timerHandler, settingsHandler, eventsHandler, dxHandler);
                var playerFightHandler = new PlayerFightHandler(loggingHandler, eventsHandler, settingsHandler, browserHandler, floatingDamageInfoHandler, utilsHandler, camerasHandler, timerHandler);
                new AntiCheatHandler(loggingHandler, playerFightHandler);
                var mapLimitHandler = new MapLimitHandler(loggingHandler, settingsHandler, remoteEventsSender, eventsHandler, dxHandler, timerHandler);

                var lobbyHandler = new LobbyHandler(loggingHandler, browserHandler, playerFightHandler, eventsHandler, settingsHandler, bindsHandler, remoteEventsSender, dxHandler,
                    timerHandler, utilsHandler, camerasHandler, cursorHandler, dataSyncHandler, mapLimitHandler);
                new GangWindowHandler(loggingHandler, browserHandler, cursorHandler, eventsHandler, bindsHandler);
                new DamageHandler(browserHandler, remoteEventsSender, playerFightHandler, lobbyHandler, eventsHandler);
                var scoreboardHandler = new ScoreboardHandler(loggingHandler, dxHandler, settingsHandler, lobbyHandler, timerHandler, remoteEventsSender, eventsHandler, bindsHandler);
                var chatHandler = new ChatHandler(loggingHandler, browserHandler, bindsHandler, remoteEventsSender, eventsHandler);
                new CommandsHandler(loggingHandler, chatHandler, settingsHandler, lobbyHandler, playerFightHandler, camerasHandler, remoteEventsSender, utilsHandler);
                new ShirtTeamColorsHandler(loggingHandler, lobbyHandler, dataSyncHandler, eventsHandler);

                var workaroundsHandler = new WorkaroundsHandler(loggingHandler, utilsHandler);
                new AFKCheckHandler(loggingHandler, eventsHandler, settingsHandler, remoteEventsSender, playerFightHandler, timerHandler, dxHandler);

                var nametagsHandler = new NametagsHandler(loggingHandler, camerasHandler, settingsHandler, utilsHandler, playerFightHandler);

                new RankingHandler(loggingHandler, camerasHandler, utilsHandler, settingsHandler, cursorHandler, browserHandler, nametagsHandler, deathHandler, eventsHandler, timerHandler);
                new MapCreatorHandler(loggingHandler, bindsHandler, instructionalButtonHandler, settingsHandler, utilsHandler, camerasHandler, cursorHandler,
                    browserHandler, dxHandler, remoteEventsSender, eventsHandler, lobbyHandler, timerHandler, dataSyncHandler, deathHandler);
                new InfosHandler(loggingHandler, browserHandler, eventsHandler);
                new WeaponStatsHandler(loggingHandler, remoteEventsSender);

                loggingHandler.LogInfo("Services successfully initialized", "Services.Initialize", true);
            }
            catch (Exception ex)
            {
                loggingHandler.LogError(ex);
            }
        }
    }
}
