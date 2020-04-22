using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Draw;

namespace TDS_Client.Handler.MapCreator
{
    public class MapCreatorFootHandler
    {
        private readonly IModAPI ModAPI;
        private readonly CamerasHandler _camerasHandler;
        private readonly InstructionalButtonHandler _instructionalButtonHandler;
        private readonly SettingsHandler _settingsHandler;

        public MapCreatorFootHandler(IModAPI modAPI, CamerasHandler camerasHandler, InstructionalButtonHandler instructionalButtonHandler, SettingsHandler settingsHandler)
        {
            ModAPI = modAPI;
            _camerasHandler = camerasHandler;
            _instructionalButtonHandler = instructionalButtonHandler;
            _settingsHandler = settingsHandler;
        }

        public void Start(bool addInstructionalButtom = true)
        {
            var player = ModAPI.LocalPlayer;

            if (!(_camerasHandler.FreeCam is null))
            {
                var cam = _camerasHandler.FreeCam;
                player.Position = cam.Position;
                player.Rotation = cam.Rotation;
            }
            player.FreezePosition(false);
            player.SetVisible(true);
            player.SetCollision(true, true);

            _camerasHandler.RenderBack();

            if (addInstructionalButtom)
                _instructionalButtonHandler.Add(_settingsHandler.Language.FREECAM, "M");
        }

        public void Stop()
        {
            
        }
    }
}
