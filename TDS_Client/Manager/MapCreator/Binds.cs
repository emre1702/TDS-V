using RAGE.Game;
using TDS_Client.Enum;
using TDS_Client.Manager.Draw;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Manager.MapCreator
{
    class Binds
    {
        private static bool _generalBinded;

        public static void SetGeneral(bool onlyInstructionalButtons = false)
        {
            if (!_generalBinded)
            {
                BindManager.Add(EKey.M, Main.ToggleFreecam);
            }
            
            InstructionalButtonManager.Add(Settings.Language.FREECAM, "M");

            _generalBinded = true;
        }

        public static void RemoveGeneral()
        {
            if (_generalBinded)
            {
                BindManager.Remove(EKey.M, Main.ToggleFreecam);
                BindManager.Remove(EKey.F, ObjectPlacing.TogglePlaceOnGround);
            }
            
            _generalBinded = false;
        }

        public static void SetForInFreecam()
        {
            BindManager.Add(EKey.E, Freecam.KeyDown, EKeyPressState.Down);
            BindManager.Add(EKey.E, Freecam.KeyUp, EKeyPressState.Up);
            BindManager.Add(EKey.Q, Freecam.KeyDown, EKeyPressState.Down);
            BindManager.Add(EKey.Q, Freecam.KeyUp, EKeyPressState.Up);
            BindManager.Add(EKey.Delete, ObjectPlacing.DeleteHoldingObject, EKeyPressState.Down);
            BindManager.Add(Control.Attack, ObjectPlacing.LeftMouseClick, EKeyPressState.Down, OnDisabled: true);
            BindManager.Add(EKey.F, ObjectPlacing.TogglePlaceOnGround);

            var lang = Settings.Language;
            InstructionalButtonManager.Add(lang.LET_IT_FLOAT, "F");
            InstructionalButtonManager.Add(lang.DIRECTION, Control.VehicleRoof);
            InstructionalButtonManager.Add(lang.DELETE_DESCRIPTION, lang.DELETE_KEY);
            InstructionalButtonManager.Add(lang.SLOWER, Control.VehicleFlySelectPrevWeapon);
            InstructionalButtonManager.Add(lang.FASTER, Control.VehicleFlySelectNextWeapon);
            InstructionalButtonManager.Add(lang.SLOW_MODE, lang.LEFT_CTRL);
            InstructionalButtonManager.Add(lang.FAST_MODE, lang.LEFT_SHIFT);
            //InstructionalButtonManager.Add(lang.DOWN, "E");
            //InstructionalButtonManager.Add(lang.UP, "Q");
            
            InstructionalButtonManager.IsLayoutPositive = false;
        }

        public static void RemoveForInFreeCam()
        {
            BindManager.Remove(EKey.E, Freecam.KeyDown, EKeyPressState.Down);
            BindManager.Remove(EKey.E, Freecam.KeyUp, EKeyPressState.Up);
            BindManager.Remove(EKey.Q, Freecam.KeyDown, EKeyPressState.Down);
            BindManager.Remove(EKey.Q, Freecam.KeyUp, EKeyPressState.Up);
            BindManager.Remove(Control.Attack, ObjectPlacing.LeftMouseClick);
            BindManager.Remove(EKey.F, ObjectPlacing.TogglePlaceOnGround);
        }
    }
}
