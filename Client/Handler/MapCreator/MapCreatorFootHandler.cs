using TDS.Client.Handler.Deathmatch;
using TDS.Client.Handler.Draw;

namespace TDS.Client.Handler.MapCreator
{
    public class MapCreatorFootHandler
    {
        private readonly CamerasHandler _camerasHandler;
        private readonly DeathHandler _deathHandler;
        private readonly InstructionalButtonHandler _instructionalButtonHandler;

        private readonly SettingsHandler _settingsHandler;

        public MapCreatorFootHandler(CamerasHandler camerasHandler, InstructionalButtonHandler instructionalButtonHandler,
            SettingsHandler settingsHandler, DeathHandler deathHandler)
        {
            _camerasHandler = camerasHandler;
            _instructionalButtonHandler = instructionalButtonHandler;
            _settingsHandler = settingsHandler;
            _deathHandler = deathHandler;
        }

        public void Start(bool addInstructionalButtom = true)
        {
            var player = RAGE.Elements.Player.LocalPlayer;

            if (!(_camerasHandler.FreeCam is null))
            {
                var cam = _camerasHandler.FreeCam;
                player.Position = cam.Position;
                var camRot = cam.Rotation;
                player.SetRotation(camRot.X, camRot.Y, camRot.Z, 2, true);
            }
            player.FreezePosition(false);
            player.SetVisible(true, true);
            player.SetAlpha(255, false);
            player.SetCollision(true, true);
            _deathHandler.PlayerSpawn();

            _camerasHandler.RenderBack();

            if (addInstructionalButtom)
                _instructionalButtonHandler.Add(_settingsHandler.Language.FREECAM, "M");
        }

        public void Stop()
        {
        }
    }
}
