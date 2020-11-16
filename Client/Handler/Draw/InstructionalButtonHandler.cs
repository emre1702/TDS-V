using RAGE.Game;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TDS.Client.Data.Defaults;
using TDS.Client.Data.Interfaces;
using TDS.Client.Handler.Entities.Draw.Scaleform;
using TDS.Client.Handler.Events;
using TDS.Shared.Data.Models;
using static RAGE.Events;

namespace TDS.Client.Handler.Draw
{
    public class InstructionalButtonHandler : ServiceBase
    {
        private readonly List<InstructionalButton> _allButtons = new List<InstructionalButton>();

        private readonly List<InstructionalButton> _buttons = new List<InstructionalButton>();

        private readonly List<InstructionalButton> _persistentButtons = new List<InstructionalButton>();

        private readonly BasicScaleform _scaleform;

        private readonly SettingsHandler _settingsHandler;

        private Color _backgroundColor;

        private bool _isActive = true;

        private bool _isLayoutPositive;

        public InstructionalButtonHandler(LoggingHandler loggingHandler, EventsHandler eventsHandler, SettingsHandler settingsHandler)
            : base(loggingHandler)
        {
            _settingsHandler = settingsHandler;

            _scaleform = new BasicScaleform(ScaleformFunction.INSTRUCTIONAL_BUTTONS);

            Add("Cursor", _settingsHandler.Language.END_KEY, true);

            eventsHandler.LanguageChanged += EventsHandler_LanguageChanged;
            eventsHandler.LobbyJoined += EventsHandler_LobbyJoined;
            eventsHandler.LobbyLeft += _ => Reset();

            Reset();
        }

        public Color BackgroundColor
        {
            get => _backgroundColor;
            set => SetBackgroundColor(value);
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                if (value)
                {
                    Redraw();
                    Tick += OnTick;
                }
                else
                    Tick -= OnTick;
            }
        }

        /// <summary>
        /// Negative =&gt; One Row | Positive =&gt; Row per button
        /// </summary>
        public bool IsLayoutPositive
        {
            get => _isLayoutPositive;
            set => SetLayout(value);
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

        public void Redraw()
        {
            if (IsActive)
                _scaleform.Call("DRAW_INSTRUCTIONAL_BUTTONS", _isLayoutPositive ? 1 : -1);
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

        public void SetBackgroundColor(Color color)
        {
            _backgroundColor = color;
            _scaleform.Call("SET_BACKGROUND_COLOUR", (int)color.R, (int)color.G, (int)color.B, (int)color.A);
            Redraw();
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

        public void SetLayout(bool positive)
        {
            _isLayoutPositive = positive;
        }

        private void EventsHandler_LanguageChanged(ILanguage lang, bool beforeLogin)
        {
        }

        private void EventsHandler_LobbyJoined(SyncedLobbySettings settings)
        {
            if (settings.InLobbyWithMaps)
                Add("Map-Voting", "F3");
        }

        private void OnTick(List<TickNametagData> _)
        {
            _scaleform.RenderFullscreen();
        }
    }
}
