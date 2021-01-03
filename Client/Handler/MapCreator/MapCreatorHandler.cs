using RAGE;
using System;
using TDS.Client.Data.Defaults;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Deathmatch;
using TDS.Client.Handler.Draw;
using TDS.Client.Handler.Draw.Dx;
using TDS.Client.Handler.Events;
using TDS.Client.Handler.Lobby;
using TDS.Client.Handler.Sync;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models;
using TDS.Shared.Data.Models.GTA;
using TDS.Shared.Default;

namespace TDS.Client.Handler.MapCreator
{
    public class MapCreatorHandler : ServiceBase
    {
        private readonly BrowserHandler _browserHandler;
        private readonly CamerasHandler _camerasHandler;
        private readonly InstructionalButtonHandler _instructionalButtonHandler;
        private readonly RemoteEventsSender _remoteEventsSender;

        public MapCreatorHandler(LoggingHandler loggingHandler, BindsHandler bindsHandler, InstructionalButtonHandler instructionalButtonHandler,
            SettingsHandler settingsHandler, UtilsHandler utilsHandler, CamerasHandler camerasHandler, CursorHandler cursorHandler, BrowserHandler browserHandler,
            DxHandler dxHandler, RemoteEventsSender remoteEventsSender, EventsHandler eventsHandler, LobbyHandler lobbyHandler,
            TimerHandler timerHandler, DataSyncHandler dataSyncHandler, DeathHandler deathHandler)
            : base(loggingHandler)
        {
            _browserHandler = browserHandler;
            _instructionalButtonHandler = instructionalButtonHandler;
            _remoteEventsSender = remoteEventsSender;
            _camerasHandler = camerasHandler;

            Draw = new MapCreatorDrawHandler(utilsHandler);
            Foot = new MapCreatorFootHandler(camerasHandler, instructionalButtonHandler, settingsHandler, deathHandler);

            var clickedMarkerStorer = new ClickedMarkerStorer();

            Objects = new MapCreatorObjectsHandler(loggingHandler, camerasHandler, lobbyHandler, eventsHandler, browserHandler, settingsHandler, remoteEventsSender, dxHandler, timerHandler);
            Sync = new MapCreatorSyncHandler(Objects, remoteEventsSender, eventsHandler, browserHandler, lobbyHandler, dataSyncHandler);
            ObjectsLoading = new ObjectsLoadingHelper(loggingHandler, utilsHandler, settingsHandler);
            ObjectsPreview = new MapCreatorObjectsPreviewHandler(ObjectsLoading, camerasHandler, utilsHandler, browserHandler);
            VehiclePreview = new MapCreatorVehiclesPreviewHandler(camerasHandler, utilsHandler, browserHandler);
            ObjectPlacing = new MapCreatorObjectPlacingHandler(loggingHandler, Draw, Objects, cursorHandler, browserHandler, lobbyHandler, settingsHandler,
                camerasHandler, instructionalButtonHandler, utilsHandler, ObjectsPreview, VehiclePreview, Sync, eventsHandler, clickedMarkerStorer);
            Marker = new MapCreatorMarkerHandler(loggingHandler, utilsHandler, dxHandler, camerasHandler, browserHandler, Draw, ObjectPlacing, Sync, clickedMarkerStorer);
            Freecam = new MapCreatorFreecamHandler(loggingHandler, camerasHandler, utilsHandler, instructionalButtonHandler, cursorHandler, browserHandler,
                Foot, Marker, ObjectPlacing, eventsHandler);
            Binds = new MapCreatorBindsHandler(bindsHandler, instructionalButtonHandler, settingsHandler, Freecam, ObjectPlacing, eventsHandler);

            eventsHandler.LobbyJoined += EventsHandler_LobbyJoined;
            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;

            RAGE.Events.Add(ToServerEvent.RemoveMap, OnRemoveMapMethod);
            RAGE.Events.Add(ToClientEvent.MapCreatorStartNewMap, _ => StartNewMap());
            RAGE.Events.Add(FromBrowserEvent.TeleportToXY, OnTeleportToXYMethod);
            RAGE.Events.Add(FromBrowserEvent.TeleportToPositionRotation, OnTeleportToPositionRotationMethod);
        }

        public MapCreatorBindsHandler Binds { get; }
        public MapCreatorDrawHandler Draw { get; }
        public MapCreatorFootHandler Foot { get; }
        public MapCreatorFreecamHandler Freecam { get; }
        public MapCreatorMarkerHandler Marker { get; }
        public MapCreatorObjectPlacingHandler ObjectPlacing { get; }
        public MapCreatorObjectsHandler Objects { get; }
        public ObjectsLoadingHelper ObjectsLoading { get; }
        public MapCreatorObjectsPreviewHandler ObjectsPreview { get; }
        public MapCreatorSyncHandler Sync { get; }
        public MapCreatorVehiclesPreviewHandler VehiclePreview { get; }

        public void Start()
        {
            RAGE.Game.Cam.DoScreenFadeIn(100);
            _browserHandler.Angular.ToggleMapCreator(true);
            Freecam.ToggleFreecam();
            Objects.Start();
            Marker.Start();
            Sync.Start();
        }

        public void StartNewMap()
        {
            Objects.Stop();
            Objects.Start();
        }

        public void Stop()
        {
            _instructionalButtonHandler.Reset();
            Binds.RemoveGeneral();
            Freecam.Stop();
            Foot.Start(false);
            _browserHandler.Angular.ToggleMapCreator(false);
            Objects.Stop();
            Marker.Stop();
            Sync.Stop();
            ObjectsPreview.Stop();
            VehiclePreview.Stop();
        }

        private void EventsHandler_LobbyJoined(SyncedLobbySettings settings)
        {
            if (settings.Type != LobbyType.MapCreateLobby)
                return;

            Start();
        }

        private void EventsHandler_LobbyLeft(SyncedLobbySettings settings)
        {
            if (settings.Type != LobbyType.MapCreateLobby)
                return;

            Stop();
        }

        private void OnRemoveMapMethod(object[] args)
        {
            int mapId = Convert.ToInt32(args[0]);
            if (mapId == 0 || _remoteEventsSender.Send(ToServerEvent.RemoveMap, mapId))
            {
                Objects.Stop();
                Objects.Start();
            }
            else
                _browserHandler.Angular.ShowCooldown();
        }

        private void OnTeleportToPositionRotationMethod(object[] args)
        {
            float x = Convert.ToSingle(args[0]);
            float y = Convert.ToSingle(args[1]);
            float z = Convert.ToSingle(args[2]);
            float rot = Convert.ToSingle(args[3]);
            if (_camerasHandler.ActiveCamera != null)
            {
                _camerasHandler.ActiveCamera.Position = new Vector3(x, y, z);
                _camerasHandler.ActiveCamera.Rotation = new Vector3(0, 0, rot);
            }
            RAGE.Elements.Player.LocalPlayer.Position = new Vector3(x, y, z);
            RAGE.Elements.Player.LocalPlayer.SetHeading(rot);
        }

        private void OnTeleportToXYMethod(object[] args)
        {
            float x = Convert.ToSingle(args[0]);
            float y = Convert.ToSingle(args[1]);
            float z = 0;
            RAGE.Game.Misc.GetGroundZFor3dCoord(x, y, 9000, ref z, false);
            RAGE.Elements.Player.LocalPlayer.Position = new Vector3(x, y, z + 0.3f);
        }
    }
}