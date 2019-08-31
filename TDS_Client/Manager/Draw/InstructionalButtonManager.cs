using RAGE.Game;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Instance.Draw.Scaleform;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Manager.Draw
{
    static class InstructionalButtonManager
    {
        public static bool IsActive 
        { 
            get => _isActive;
            set
            {
                _isActive = value;
                if (value)
                    Redraw();
            }
        }

        /// <summary>
        /// Negative => One Row | Positive => Row per button
        /// </summary>
        public static bool IsLayoutPositive
        {
            get => _isLayoutPositive;
            set => SetLayout(value);
        }

        public static Color BackgroundColor
        {
            get => _backgroundColor;
            set => SetBackgroundColor(value);
        }

        private static readonly BasicScaleform _scaleform = new BasicScaleform("INSTRUCTIONAL_BUTTONS");
        private static List<InstructionalButton> _buttons = new List<InstructionalButton>();
        private static bool _isLayoutPositive;
        private static Color _backgroundColor;
        private static bool _isActive;

        static InstructionalButtonManager() 
        {
            TickManager.Add(OnTick, () => IsActive);
        }

        private static void OnTick()
        {
            _scaleform.RenderFullscreen();
        }

        public static void Add(string title, string control)
        {
            _buttons.Add(new InstructionalButton(title, control, _buttons.Count));
        }

        public static void Add(string title, Control control)
        {
            _buttons.Add(new InstructionalButton(title, control, _buttons.Count));
        }

        public static void SetDataSlot(int slot, string control, string title)
        {
            _scaleform.Call("SET_DATA_SLOT", slot, control, title);
            Redraw();
        }

        public static void SetDataSlot(int slot, Control control, string title)
        {
            _scaleform.Call("SET_DATA_SLOT", slot, (int)control, title);
            Redraw();
        }

        public static void Redraw()
        {
            if (IsActive)
                _scaleform.Call("DRAW_INSTRUCTIONAL_BUTTONS", _isLayoutPositive ? 1 : -1);
        }

        public static void SetLayout(bool positive)
        {
            _isLayoutPositive = positive;
        }

        public static void SetBackgroundColor(Color color)
        {
            _backgroundColor = color;
            _scaleform.Call("SET_BACKGROUND_COLOUR", (int)color.R, (int)color.G, (int)color.B, (int)color.A);
            Redraw();
        }

        public static void Reset()
        {
            _scaleform.Call("CLEAR_ALL");
            _scaleform.Call("TOGGLE_MOUSE_BUTTONS", 0);
            _scaleform.Call("CREATE_CONTAINER");
            _scaleform.Call("SET_CLEAR_SPACE", 100);
            _buttons = new List<InstructionalButton>();

            AddDefaultButtons();
        }

        private static void AddDefaultButtons()
        {
            Add("Cursor", Settings.Language.END_KEY);
            Add("Userpanel", "U");
            Add("Voice", Control.PushToTalk);
        }
    }
}
