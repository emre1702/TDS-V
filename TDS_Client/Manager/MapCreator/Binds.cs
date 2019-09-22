using RAGE.Game;
using TDS_Client.Enum;
using TDS_Client.Manager.Draw;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Manager.MapCreator
{
    class Binds
    {
        public static void SetGeneral()
        {
            BindManager.Add(EKey.M, Main.ToggleFreecam);
            BindManager.Add(EKey.F, ObjectPlacing.TogglePlaceOnGround);

            InstructionalButtonManager.Add(Settings.Language.TOGGLE_FREECAM, "M");
            InstructionalButtonManager.Add(Settings.Language.TOGGLE_ON_GROUND, "F");
        }

        public static void RemoveGeneral()
        {
            BindManager.Remove(EKey.M, Main.ToggleFreecam);
            BindManager.Remove(EKey.F, ObjectPlacing.TogglePlaceOnGround);

            InstructionalButtonManager.Reset();
        }

        public static void SetForInFreecam()
        {
            BindManager.Add(EKey.E, Freecam.KeyDown, EKeyPressState.Down);
            BindManager.Add(EKey.E, Freecam.KeyUp, EKeyPressState.Up);
            BindManager.Add(EKey.Q, Freecam.KeyDown, EKeyPressState.Down);
            BindManager.Add(EKey.Q, Freecam.KeyUp, EKeyPressState.Up);
            BindManager.Add(EKey.Delete, ObjectPlacing.DeleteHoldingObject, EKeyPressState.Down);
            BindManager.Add(Control.Attack, ObjectPlacing.LeftMouseClick, EKeyPressState.Down, OnDisabled: true);

            var lang = Settings.Language;
            InstructionalButtonManager.Add(lang.DELETE_DESCRIPTION, lang.DELETE_KEY);
            InstructionalButtonManager.Add(lang.SLOWER, Control.VehicleFlySelectPrevWeapon);
            InstructionalButtonManager.Add(lang.FASTER, Control.VehicleFlySelectNextWeapon);
            InstructionalButtonManager.Add(lang.SLOW_MODE, lang.LEFT_CTRL);
            InstructionalButtonManager.Add(lang.FAST_MODE, lang.LEFT_SHIFT);
            //InstructionalButtonManager.Add(lang.DOWN, "E");
            //InstructionalButtonManager.Add(lang.UP, "Q");
            InstructionalButtonManager.Add(lang.DIRECTION, Control.VehicleRoof);
            InstructionalButtonManager.IsLayoutPositive = false;
        }

        public static void RemoveForInFreeCam()
        {
            BindManager.Remove(EKey.E, Freecam.KeyDown, EKeyPressState.Down);
            BindManager.Remove(EKey.E, Freecam.KeyUp, EKeyPressState.Up);
            BindManager.Remove(EKey.Q, Freecam.KeyDown, EKeyPressState.Down);
            BindManager.Remove(EKey.Q, Freecam.KeyUp, EKeyPressState.Up);
            BindManager.Remove(Control.Attack, ObjectPlacing.LeftMouseClick);

            InstructionalButtonManager.Reset();
        }
    }
}
