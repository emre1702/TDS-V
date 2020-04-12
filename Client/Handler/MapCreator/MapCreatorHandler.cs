using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Draw;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Lobby;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;

namespace TDS_Client.Handler.MapCreator
{
    public class MapCreatorHandler
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

        private readonly IModAPI _modAPI;
        private readonly BrowserHandler _browserHandler;
        private readonly InstructionalButtonHandler _instructionalButtonHandler;

        public MapCreatorHandler(IModAPI modAPI, BindsHandler bindsHandler, InstructionalButtonHandler instructionalButtonHandler, SettingsHandler settingsHandler, UtilsHandler utilsHandler,
            CamerasHandler camerasHandler, CursorHandler cursorHandler, BrowserHandler browserHandler, DxHandler dxHandler,
            RemoteEventsSender remoteEventsSender, Serializer serializer, EventsHandler eventsHandler, LobbyHandler lobbyHandler)
        {
            _modAPI = modAPI;
            _browserHandler = browserHandler;
            _instructionalButtonHandler = instructionalButtonHandler;

            Draw = new MapCreatorDrawHandler(modAPI, utilsHandler);
            Foot = new MapCreatorFootHandler(modAPI, camerasHandler, instructionalButtonHandler, settingsHandler);
            Marker = new MapCreatorMarkerHandler(modAPI, utilsHandler, dxHandler, camerasHandler, browserHandler, Draw);
            Objects = new MapCreatorObjectsHandler(modAPI, camerasHandler, lobbyHandler, eventsHandler);
            Sync = new MapCreatorSyncHandler(modAPI, Objects, remoteEventsSender, serializer, eventsHandler);
            ObjectsLoading = new ObjectsLoadingHelper(modAPI, utilsHandler, settingsHandler);
            ObjectsPreview = new MapCreatorObjectsPreviewHandler(modAPI, ObjectsLoading, camerasHandler, utilsHandler);
            VehiclePreview = new MapCreatorVehiclesPreviewHandler(modAPI, camerasHandler, utilsHandler);
            ObjectPlacing = new MapCreatorObjectPlacingHandler(modAPI, Marker, Draw, Objects, cursorHandler, browserHandler, lobbyHandler, settingsHandler, remoteEventsSender, camerasHandler,
               instructionalButtonHandler, utilsHandler, ObjectsPreview, VehiclePreview, Sync, eventsHandler);
            Freecam = new MapCreatorFreecamHandler(modAPI, camerasHandler, utilsHandler, instructionalButtonHandler, cursorHandler, browserHandler, Foot, Marker, ObjectPlacing, eventsHandler);
            Binds = new MapCreatorBindsHandler(bindsHandler, instructionalButtonHandler, settingsHandler, Freecam, ObjectPlacing, eventsHandler);
            
            eventsHandler.LobbyJoined += EventsHandler_LobbyJoined;
            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
        }

        public void Start()
        {
            _modAPI.Cam.DoScreenFadeIn(100);
            _browserHandler.Angular.ToggleMapCreator(true);
            _browserHandler.Angular.ToggleFreeroam(true);
            Freecam.ToggleFreecam();
            Objects.Start();
            Marker.Start();
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
            ObjectsPreview.Stop();
            VehiclePreview.Stop();
        }

        public void StartNewMap()
        {
            Objects.Stop();
            Objects.Start();
        }

        private void EventsHandler_LobbyJoined(SyncedLobbySettingsDto settings)
        {
            if (settings.Type != LobbyType.MapCreateLobby)
                return;

            Start();
        }

        private void EventsHandler_LobbyLeft(SyncedLobbySettingsDto settings)
        {
            if (settings.Type != LobbyType.MapCreateLobby)
                return;

            Stop();
        }
    }
}
