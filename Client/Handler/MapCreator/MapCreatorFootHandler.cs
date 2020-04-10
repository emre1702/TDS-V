using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Draw;

namespace TDS_Client.Handler.MapCreator
{
    public class MapCreatorFootHandler
    {
        private readonly IModAPI _modAPI;
        private readonly CamerasHandler _camerasHandler;
        private readonly InstructionalButtonHandler _instructionalButtonHandler;
        private readonly SettingsHandler _settingsHandler;

        public MapCreatorFootHandler(IModAPI modAPI, CamerasHandler camerasHandler, InstructionalButtonHandler instructionalButtonHandler, SettingsHandler settingsHandler)
        {
            _modAPI = modAPI;
            _camerasHandler = camerasHandler;
            _instructionalButtonHandler = instructionalButtonHandler;
            _settingsHandler = settingsHandler;
        }

        public void Start(bool addInstructionalButtom = true)
        {
            var player = _modAPI.LocalPlayer;

            if (!(_camerasHandler.FreeCam is null))
            {
                var cam = _camerasHandler.FreeCam;
                player.Position = cam.Position;
                player.Rotation = cam.Rotation;
            }
            player.FreezePosition(false);
            player.SetVisible(true, false);
            player.SetCollision(true, true);

            _camerasHandler.RenderBack();

            if (addInstructionalButtom)
                _instructionalButtonHandler.Add(_settingsHandler.Language.FREECAM, "M");
        }

        public void Stop()
        {
            var player = _modAPI.LocalPlayer;
            player.FreezePosition(true);
            player.SetVisible(false, false);
            player.SetCollision(false, false);
        }
    }
}
