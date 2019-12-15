using RAGE.Game;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        private static readonly List<InstructionalButton> _allButtons = new List<InstructionalButton>();
        private static readonly List<InstructionalButton> _buttons = new List<InstructionalButton>();
        private static readonly List<InstructionalButton> _persistentButtons = new List<InstructionalButton>();
        private static bool _isLayoutPositive;
        private static Color _backgroundColor;
        private static bool _isActive = true;

        static InstructionalButtonManager() 
        {
            AddDefaultButtons();
            TickManager.Add(OnTick, () => IsActive);
        }

        private static void OnTick()
        {
            _scaleform.RenderFullscreen();
        }

        public static InstructionalButton Add(string title, string control, bool persistent = false)
        {
            var button = _allButtons.FirstOrDefault(b => b.OriginalControlString == control);
            if (button != null)
            {
                button.Title = title;
                if (persistent && !_persistentButtons.Contains(button))
                {
                    _buttons.Remove(button);
                    _persistentButtons.Add(button);
                }
            }
            else
            {
                button = new InstructionalButton(title, control, _allButtons.Count);
                List<InstructionalButton> theList = persistent ? _persistentButtons : _buttons;
                theList.Add(button);
                _allButtons.Add(button);
            }

            return button;
        }

        public static InstructionalButton Add(string title, Control control, bool persistent = false)
        {
            var button = _allButtons.FirstOrDefault(b => b.ControlEnum == control);
            if (button != null)
            {
                button.Title = title;
                if (persistent && !_persistentButtons.Contains(button))
                {
                    _buttons.Remove(button);
                    _persistentButtons.Add(button);
                }
            }
            else
            {
                button = new InstructionalButton(title, control, _allButtons.Count);
                List<InstructionalButton> theList = persistent ? _persistentButtons : _buttons;
                theList.Add(button);
                _allButtons.Add(button);
            }

            return button;
        }

        public static void Remove(InstructionalButton button)
        {
            _allButtons.Remove(button);
            _persistentButtons.Remove(button);
            _buttons.Remove(button);

            IsActive = false;
            for (int i = button.Slot; i < _allButtons.Count; ++i)
            {
                var buttonToFixSlot = _allButtons[i];
                buttonToFixSlot.Slot = i;
            }
            IsActive = true;
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
            _buttons.Clear();
            _allButtons.Clear();

            IsActive = false;
            for (int i = 0; i < _persistentButtons.Count; ++i)
            {
                var button = _persistentButtons[i];
                button.SetSlot(i);
                _allButtons.Add(button);
            }
            IsActive = true;
        }

        private static void AddDefaultButtons()
        {
            Add("Cursor", Settings.Language.END_KEY);
            Add("Userpanel", "U");
        }
    }
}
