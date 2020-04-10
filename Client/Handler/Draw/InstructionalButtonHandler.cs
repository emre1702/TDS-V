using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Entities.Draw.Scaleform;
using TDS_Client.Handler.Events;

namespace TDS_Client.Handler.Draw
{
    public class InstructionalButtonHandler
    {
        public bool IsActive
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
        public bool IsLayoutPositive
        {
            get => _isLayoutPositive;
            set => SetLayout(value);
        }

        public Color BackgroundColor
        {
            get => _backgroundColor;
            set => SetBackgroundColor(value);
        }

        private readonly BasicScaleform _scaleform;
        private readonly List<InstructionalButton> _allButtons = new List<InstructionalButton>();
        private readonly List<InstructionalButton> _buttons = new List<InstructionalButton>();
        private readonly List<InstructionalButton> _persistentButtons = new List<InstructionalButton>();
        private bool _isLayoutPositive;
        private Color _backgroundColor;
        private bool _isActive = true;

        private readonly InstructionalButton _cursorButton;
        private readonly InstructionalButton _userpanelButton;

        private readonly SettingsHandler _settingsHandler;

        public InstructionalButtonHandler(IModAPI modAPI, EventsHandler eventsHandler, SettingsHandler settingsHandler)
        {
            _settingsHandler = settingsHandler;

            _scaleform = new BasicScaleform(ScaleformFunction.INSTRUCTIONAL_BUTTONS, modAPI);

            _cursorButton = Add("Cursor", _settingsHandler.Language.END_KEY, true);
            _userpanelButton = Add("Userpanel", "U", true);

            modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(OnTick, () => IsActive));

            eventsHandler.LanguageChanged += EventsHandler_LanguageChanged;
        }

        private void OnTick(ulong currentMs)
        {
            _scaleform.RenderFullscreen();
        }

        public InstructionalButton Add(string title, string control, bool persistent = false)
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
                button = new InstructionalButton(title, control, _allButtons.Count, this);
                List<InstructionalButton> theList = persistent ? _persistentButtons : _buttons;
                theList.Add(button);
                _allButtons.Add(button);
            }

            return button;
        }

        public InstructionalButton Add(string title, Control control, bool persistent = false)
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
                button = new InstructionalButton(title, control, _allButtons.Count, this);
                List<InstructionalButton> theList = persistent ? _persistentButtons : _buttons;
                theList.Add(button);
                _allButtons.Add(button);
            }

            return button;
        }

        public void Remove(InstructionalButton button)
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

        public void SetDataSlot(int slot, string control, string title)
        {
            _scaleform.Call("SET_DATA_SLOT", slot, control, title);
            Redraw();
        }

        public void SetDataSlot(int slot, Control control, string title)
        {
            _scaleform.Call("SET_DATA_SLOT", slot, (int)control, title);
            Redraw();
        }

        public void Redraw()
        {
            if (IsActive)
                _scaleform.Call("DRAW_INSTRUCTIONAL_BUTTONS", _isLayoutPositive ? 1 : -1);
        }

        public void SetLayout(bool positive)
        {
            _isLayoutPositive = positive;
        }

        public void SetBackgroundColor(Color color)
        {
            _backgroundColor = color;
            _scaleform.Call("SET_BACKGROUND_COLOUR", (int)color.R, (int)color.G, (int)color.B, (int)color.A);
            Redraw();
        }

        public void Reset()
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

        private void EventsHandler_LanguageChanged(ILanguage lang, bool beforeLogin)
        {

        }
    }
}
