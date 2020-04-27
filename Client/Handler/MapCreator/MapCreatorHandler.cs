using System;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Draw;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Lobby;
using TDS_Client.Handler.Sync;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Default;

namespace TDS_Client.Handler.MapCreator
{
    public class MapCreatorHandler : ServiceBase
    {
        public MapCreatorBindsHandler Binds { get; }
        public MapCreatorDrawHandler Draw { get; }
        public MapCreatorFootHandler Foot { get; }
        public MapCreatorFreecamHandler Freecam { get; }
        public MapCreatorMarkerHandler Marker { get; }
        public MapCreatorObjectsHandler Objects { get; }
        public ObjectsLoadingHelper ObjectsLoading { get; }
        public MapCreatorObjectsPreviewHandler ObjectsPreview { get; }
        public MapCreatorObjectPlacingHandler ObjectPlacing { get; }
        public MapCreatorSyncHandler Sync { get; }
        public MapCreatorVehiclesPreviewHandler VehiclePreview { get; }

        private readonly BrowserHandler _browserHandler;
        private readonly InstructionalButtonHandler _instructionalButtonHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly CamerasHandler _camerasHandler;

        public MapCreatorHandler(IModAPI modAPI, LoggingHandler loggingHandler, BindsHandler bindsHandler, InstructionalButtonHandler instructionalButtonHandler, 
            SettingsHandler settingsHandler, UtilsHandler utilsHandler, CamerasHandler camerasHandler, CursorHandler cursorHandler, BrowserHandler browserHandler, 
            DxHandler dxHandler, RemoteEventsSender remoteEventsSender, Serializer serializer, EventsHandler eventsHandler, LobbyHandler lobbyHandler, 
            TimerHandler timerHandler, DataSyncHandler dataSyncHandler)
            : base(modAPI, loggingHandler)
        {
            _browserHandler = browserHandler;
            _instructionalButtonHandler = instructionalButtonHandler;
            _remoteEventsSender = remoteEventsSender;
            _camerasHandler = camerasHandler;

            Draw = new MapCreatorDrawHandler(modAPI, utilsHandler);
            Foot = new MapCreatorFootHandler(modAPI, camerasHandler, instructionalButtonHandler, settingsHandler);
            
            var clickedMarkerStorer = new ClickedMarkerStorer();

            Objects = new MapCreatorObjectsHandler(modAPI, loggingHandler, camerasHandler, lobbyHandler, eventsHandler, browserHandler, serializer);
            Sync = new MapCreatorSyncHandler(modAPI, Objects, remoteEventsSender, serializer, eventsHandler, browserHandler, lobbyHandler, dataSyncHandler);
            ObjectsLoading = new ObjectsLoadingHelper(modAPI, loggingHandler, utilsHandler, settingsHandler);
            ObjectsPreview = new MapCreatorObjectsPreviewHandler(modAPI, ObjectsLoading, camerasHandler, utilsHandler, browserHandler);
            VehiclePreview = new MapCreatorVehiclesPreviewHandler(modAPI, camerasHandler, utilsHandler, browserHandler);
            ObjectPlacing = new MapCreatorObjectPlacingHandler(modAPI, loggingHandler, Draw, Objects, cursorHandler, browserHandler, lobbyHandler, settingsHandler, 
                remoteEventsSender, camerasHandler,
                instructionalButtonHandler, utilsHandler, ObjectsPreview, VehiclePreview, Sync, eventsHandler, dxHandler, timerHandler, clickedMarkerStorer);
            Marker = new MapCreatorMarkerHandler(modAPI, loggingHandler, utilsHandler, dxHandler, camerasHandler, browserHandler, Draw, ObjectPlacing, Sync, clickedMarkerStorer);
            Freecam = new MapCreatorFreecamHandler(modAPI, loggingHandler, camerasHandler, utilsHandler, instructionalButtonHandler, cursorHandler, browserHandler, 
                Foot, Marker, ObjectPlacing, eventsHandler);
            Binds = new MapCreatorBindsHandler(bindsHandler, instructionalButtonHandler, settingsHandler, Freecam, ObjectPlacing, eventsHandler);
            
            eventsHandler.LobbyJoined += EventsHandler_LobbyJoined;
            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;

            ModAPI.Event.Add(ToServerEvent.RemoveMap, OnRemoveMapMethod);
            ModAPI.Event.Add(ToClientEvent.MapCreatorStartNewMap, _ => StartNewMap());
            ModAPI.Event.Add(FromBrowserEvent.TeleportToXY, OnTeleportToXYMethod);
            ModAPI.Event.Add(FromBrowserEvent.TeleportToPositionRotation, OnTeleportToPositionRotationMethod);
        }

        public void Start()
        {
            ModAPI.Cam.DoScreenFadeIn(100);
            _browserHandler.Angular.ToggleMapCreator(true);
            _browserHandler.Angular.ToggleFreeroam(true);
            Freecam.ToggleFreecam();
            Objects.Start();
            Marker.Start();
            Sync.Start();
        }

        public void Stop()
        {
            _instructionalButtonHandler.Reset();
            Binds.RemoveGeneral();
            Freecam.Stop();
            Foot.Start(false);
            _browserHandler.Angular.ToggleMapCreator(false);
            _browserHandler.Angular.ToggleFreeroam(false);
            Objects.Stop();
            Marker.Stop();
            Sync.Stop();
            ObjectsPreview.Stop();
            VehiclePreview.Stop();
        }

        public void StartNewMap()
        {
            Objects.Stop();
            Objects.Start();
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

        private void OnTeleportToXYMethod(object[] args)
        {
            float x = Convert.ToSingle(args[0]);
            float y = Convert.ToSingle(args[1]);
            float z = 0;
            ModAPI.Misc.GetGroundZFor3dCoord(x, y, 9000, ref z);
            ModAPI.LocalPlayer.Position = new Position3D(x, y, z + 0.3f);
        }

        private void OnTeleportToPositionRotationMethod(object[] args)
        {
            float x = Convert.ToSingle(args[0]);
            float y = Convert.ToSingle(args[1]);
            float z = Convert.ToSingle(args[2]);
            float rot = Convert.ToSingle(args[3]);
            if (_camerasHandler.ActiveCamera != null)
            {
                _camerasHandler.ActiveCamera.Position = new Position3D(x, y, z);
                _camerasHandler.ActiveCamera.Rotation = new Position3D(0, 0, rot);
            }
            ModAPI.LocalPlayer.Position = new Position3D(x, y, z);
            ModAPI.LocalPlayer.Heading = rot;
        }
    }
}
