using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Draw;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;

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
        public MapCreatorSyncHandler Sync { get; }

        private readonly BrowserHandler _browserHandler;
        private readonly InstructionalButtonHandler _instructionalButtonHandler;

        public MapCreatorHandler(IModAPI modAPI, BindsHandler bindsHandler, InstructionalButtonHandler instructionalButtonHandler, SettingsHandler settingsHandler, UtilsHandler utilsHandler,
            CamerasHandler camerasHandler, SpectatingHandler spectatingHandler, CursorHandler cursorHandler, BrowserHandler browserHandler, DxHandler dxHandler,
            RemoteEventsSender remoteEventsSender, Serializer serializer)
        {
            _browserHandler = browserHandler;
            _instructionalButtonHandler = instructionalButtonHandler;

            Draw = new MapCreatorDrawHandler(modAPI, utilsHandler);
            Foot = new MapCreatorFootHandler(modAPI, camerasHandler, instructionalButtonHandler, settingsHandler);
            Freecam = new MapCreatorFreecamHandler(modAPI, camerasHandler, utilsHandler, spectatingHandler, Binds, instructionalButtonHandler, settingsHandler, cursorHandler, browserHandler, Foot);
            Marker = new MapCreatorMarkerHandler(modAPI, utilsHandler, dxHandler, camerasHandler, browserHandler, Draw);
            Objects = new MapCreatorObjectsHandler(modAPI, camerasHandler, lobbyHandler);
            Binds = new MapCreatorBindsHandler(bindsHandler, instructionalButtonHandler, settingsHandler, Freecam);
            ObjectsLoading = new ObjectsLoadingHelper(modAPI, utilsHandler, settingsHandler);
            ObjectsPreview = new MapCreatorObjectsPreviewHandler(modAPI, ObjectsLoading, camerasHandler, utilsHandler);
            Sync = new MapCreatorSyncHandler(Objects, remoteEventsSender, serializer);
        }

        public void Start()
        {
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
            _browserHandler.MapCreatorObjectChoice.Stop();
        }

        

        public void StartNewMap()
        {
            Objects.Stop();
            Objects.Start();
        }
    }
}
