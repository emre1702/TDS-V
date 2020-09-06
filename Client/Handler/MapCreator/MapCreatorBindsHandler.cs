using RAGE.Game;
using TDS_Client.Data.Enums;
using TDS_Client.Handler.Draw;
using TDS_Client.Handler.Events;

namespace TDS_Client.Handler.MapCreator
{
    public class MapCreatorBindsHandler
    {
        #region Private Fields

        private readonly BindsHandler _bindsHandler;
        private readonly InstructionalButtonHandler _instructionalButtonHandler;
        private readonly MapCreatorFreecamHandler _mapCreatorFreecamHandler;
        private readonly MapCreatorObjectPlacingHandler _mapCreatorObjectPlacingHandler;
        private readonly SettingsHandler _settingsHandler;
        private bool _generalBinded;

        #endregion Private Fields

        #region Public Constructors

        public MapCreatorBindsHandler(BindsHandler bindsHandler, InstructionalButtonHandler instructionalButtonHandler, SettingsHandler settingsHandler,
            MapCreatorFreecamHandler mapCreatorFreecamHandler, MapCreatorObjectPlacingHandler mapCreatorObjectPlacingHandler, EventsHandler eventsHandler)
        {
            _bindsHandler = bindsHandler;
            _instructionalButtonHandler = instructionalButtonHandler;
            _settingsHandler = settingsHandler;
            _mapCreatorFreecamHandler = mapCreatorFreecamHandler;
            _mapCreatorObjectPlacingHandler = mapCreatorObjectPlacingHandler;

            eventsHandler.FreecamToggled += EventsHandler_FreecamToggled;
        }

        #endregion Public Constructors

        #region Public Methods

        public void RemoveForInFreeCam()
        {
            _bindsHandler.Remove(Key.E, _mapCreatorFreecamHandler.KeyDown, KeyPressState.Down);
            _bindsHandler.Remove(Key.E, _mapCreatorFreecamHandler.KeyUp, KeyPressState.Up);
            _bindsHandler.Remove(Key.Q, _mapCreatorFreecamHandler.KeyDown, KeyPressState.Down);
            _bindsHandler.Remove(Key.Q, _mapCreatorFreecamHandler.KeyUp, KeyPressState.Up);
            _bindsHandler.Remove(Control.Attack, _mapCreatorObjectPlacingHandler.LeftMouseClick);
            _bindsHandler.Remove(Key.F, _mapCreatorObjectPlacingHandler.TogglePlaceOnGround);
        }

        public void RemoveGeneral()
        {
            if (_generalBinded)
            {
                _bindsHandler.Remove(Key.M, _mapCreatorFreecamHandler.ToggleFreecam);
                _bindsHandler.Remove(Key.F, _mapCreatorObjectPlacingHandler.TogglePlaceOnGround);
            }

            _generalBinded = false;
        }

        public void SetForInFreecam()
        {
            _bindsHandler.Add(Key.E, _mapCreatorFreecamHandler.KeyDown, KeyPressState.Down);
            _bindsHandler.Add(Key.E, _mapCreatorFreecamHandler.KeyUp, KeyPressState.Up);
            _bindsHandler.Add(Key.Q, _mapCreatorFreecamHandler.KeyDown, KeyPressState.Down);
            _bindsHandler.Add(Key.Q, _mapCreatorFreecamHandler.KeyUp, KeyPressState.Up);
            _bindsHandler.Add(Key.Delete, _mapCreatorObjectPlacingHandler.DeleteHoldingObject, KeyPressState.Down);
            _bindsHandler.Add(Control.Attack, _mapCreatorObjectPlacingHandler.LeftMouseClick, KeyPressState.Down, OnDisabled: true);
            _bindsHandler.Add(Key.F, _mapCreatorObjectPlacingHandler.TogglePlaceOnGround);

            var lang = _settingsHandler.Language;
            _mapCreatorObjectPlacingHandler.AddInstructionalButton();
            _instructionalButtonHandler.Add(lang.DIRECTION, Control.VehicleRoof);
            _instructionalButtonHandler.Add(lang.DELETE_DESCRIPTION, lang.DELETE_KEY);
            _instructionalButtonHandler.Add(lang.SLOWER, Control.VehicleFlySelectPrevWeapon);
            _instructionalButtonHandler.Add(lang.FASTER, Control.VehicleFlySelectNextWeapon);
            _instructionalButtonHandler.Add(lang.SLOW_MODE, lang.LEFT_CTRL);
            _instructionalButtonHandler.Add(lang.FAST_MODE, lang.LEFT_SHIFT);
            //_instructionalButtonHandler.Add(lang.DOWN, "E");
            //_instructionalButtonHandler.Add(lang.UP, "Q");
            _instructionalButtonHandler.Add(_settingsHandler.Language.ON_FOOT, "M");

            _instructionalButtonHandler.IsLayoutPositive = false;
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

        #endregion Public Methods

        #region Private Methods

        private void EventsHandler_FreecamToggled(bool boolean)
        {
            SetGeneral();

            if (boolean)
                SetForInFreecam();
            else
                RemoveForInFreeCam();
        }

        #endregion Private Methods
    }
}
