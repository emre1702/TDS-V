using TDS_Client.Data.Enums;
using TDS_Client.Handler.Draw;

namespace TDS_Client.Handler.MapCreator
{
    public class MapCreatorBindsHandler
    {
        private bool _generalBinded;

        private readonly BindsHandler _bindsHandler;
        private readonly InstructionalButtonHandler _instructionalButtonHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly MapCreatorFreecamHandler _mapCreatorFreecamHandler;

        public MapCreatorBindsHandler(BindsHandler bindsHandler, InstructionalButtonHandler instructionalButtonHandler, SettingsHandler settingsHandler,
            MapCreatorFreecamHandler mapCreatorFreecamHandler)
        {
            _bindsHandler = bindsHandler;
            _instructionalButtonHandler = instructionalButtonHandler;
            _settingsHandler = settingsHandler;
            _mapCreatorFreecamHandler = mapCreatorFreecamHandler;
        }

        public void SetGeneral()
        {
            if (!_generalBinded)
            {
                _bindsHandler.Add(Key.M, _mapCreatorFreecamHandler.ToggleFreecam);
            }

            _instructionalButtonHandler.Add(_settingsHandler.Language.FREECAM, "M");

            _generalBinded = true;
        }

        public void RemoveGeneral()
        {
            if (_generalBinded)
            {
                _bindsHandler.Remove(Key.M, _mapCreatorFreecamHandler.ToggleFreecam);
                _bindsHandler.Remove(Key.F, MapCreatorObjectPlacingHandler.TogglePlaceOnGround);
            }

            _generalBinded = false;
        }

        public void SetForInFreecam()
        {
            _bindsHandler.Add(Key.E, _mapCreatorFreecamHandler.KeyDown, KeyPressState.Down);
            _bindsHandler.Add(Key.E, _mapCreatorFreecamHandler.KeyUp, KeyPressState.Up);
            _bindsHandler.Add(Key.Q, _mapCreatorFreecamHandler.KeyDown, KeyPressState.Down);
            _bindsHandler.Add(Key.Q, _mapCreatorFreecamHandler.KeyUp, KeyPressState.Up);
            _bindsHandler.Add(Key.Delete, MapCreatorObjectPlacingHandler.DeleteHoldingObject, KeyPressState.Down);
            _bindsHandler.Add(Control.Attack, MapCreatorObjectPlacingHandler.LeftMouseClick, KeyPressState.Down, OnDisabled: true);
            _bindsHandler.Add(Key.F, MapCreatorObjectPlacingHandler.TogglePlaceOnGround);

            var lang = _settingsHandler.Language;
            _instructionalButtonHandler.Add(lang.LET_IT_FLOAT, "F");
            _instructionalButtonHandler.Add(lang.DIRECTION, Control.VehicleRoof);
            _instructionalButtonHandler.Add(lang.DELETE_DESCRIPTION, lang.DELETE_KEY);
            _instructionalButtonHandler.Add(lang.SLOWER, Control.VehicleFlySelectPrevWeapon);
            _instructionalButtonHandler.Add(lang.FASTER, Control.VehicleFlySelectNextWeapon);
            _instructionalButtonHandler.Add(lang.SLOW_MODE, lang.LEFT_CTRL);
            _instructionalButtonHandler.Add(lang.FAST_MODE, lang.LEFT_SHIFT);
            //_instructionalButtonHandler.Add(lang.DOWN, "E");
            //_instructionalButtonHandler.Add(lang.UP, "Q");

            _instructionalButtonHandler.IsLayoutPositive = false;
        }

        public void RemoveForInFreeCam()
        {
            _bindsHandler.Remove(Key.E, _mapCreatorFreecamHandler.KeyDown, KeyPressState.Down);
            _bindsHandler.Remove(Key.E, _mapCreatorFreecamHandler.KeyUp, KeyPressState.Up);
            _bindsHandler.Remove(Key.Q, _mapCreatorFreecamHandler.KeyDown, KeyPressState.Down);
            _bindsHandler.Remove(Key.Q, _mapCreatorFreecamHandler.KeyUp, KeyPressState.Up);
            _bindsHandler.Remove(Control.Attack, MapCreatorObjectPlacingHandler.LeftMouseClick);
            _bindsHandler.Remove(Key.F, MapCreatorObjectPlacingHandler.TogglePlaceOnGround);
        }
    }
}
