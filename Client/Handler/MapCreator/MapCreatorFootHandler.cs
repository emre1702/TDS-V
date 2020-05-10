﻿using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Deathmatch;
using TDS_Client.Handler.Draw;

namespace TDS_Client.Handler.MapCreator
{
    public class MapCreatorFootHandler
    {
        private readonly IModAPI _modAPI;
        private readonly CamerasHandler _camerasHandler;
        private readonly InstructionalButtonHandler _instructionalButtonHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly DeathHandler _deathHandler;

        public MapCreatorFootHandler(IModAPI modAPI, CamerasHandler camerasHandler, InstructionalButtonHandler instructionalButtonHandler,
            SettingsHandler settingsHandler, DeathHandler deathHandler)
        {
            _modAPI = modAPI;
            _camerasHandler = camerasHandler;
            _instructionalButtonHandler = instructionalButtonHandler;
            _settingsHandler = settingsHandler;
            _deathHandler = deathHandler;
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
            player.SetVisible(true);
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
