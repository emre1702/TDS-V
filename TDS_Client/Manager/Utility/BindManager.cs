using RAGE;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Dto;
using TDS_Client.Enum;
using TDS_Client.Manager.Lobby;
using Events = RAGE.Events;
using Script = RAGE.Events.Script;

namespace TDS_Client.Manager.Utility
{
    internal class BindManager : Script
    {
        private static readonly Dictionary<ConsoleKey, List<KeyBindDto>> _bindedKeys = new Dictionary<ConsoleKey, List<KeyBindDto>>();
        private static readonly Dictionary<Control, List<ControlBindDto>> _bindedControls = new Dictionary<Control, List<ControlBindDto>>();
        private static readonly Dictionary<ConsoleKey, bool> _lastKeyDownState = new Dictionary<ConsoleKey, bool>();
        private static readonly Dictionary<Control, bool> _lastControlPressedState = new Dictionary<Control, bool>();

        public BindManager()
        {
            Events.Tick += OnTick;

            Add(ConsoleKey.End, CursorManager.ManuallyToggleCursor);
            Add(ConsoleKey.F3, MapManager.ToggleMenu);
            Add(ConsoleKey.U, Userpanel.Open);
        }

        public static void Add(ConsoleKey key, Action<ConsoleKey> method, EKeyPressState pressState = EKeyPressState.Down)
        {
            if (!_bindedKeys.ContainsKey(key))
                _bindedKeys[key] = new List<KeyBindDto>();
            _bindedKeys[key].Add(new KeyBindDto(method: method, onPressState: pressState));
        }

        public static void Add(Control control, Action<Control> method, EKeyPressState pressState = EKeyPressState.Down, bool OnEnabled = true, bool OnDisabled = false)
        {
            if (!_bindedControls.ContainsKey(control))
                _bindedControls[control] = new List<ControlBindDto>();
            _bindedControls[control].Add(new ControlBindDto(method: method, onPressState: pressState, onEnabled: OnEnabled, onDisabled: OnDisabled));
        }

        public static void Remove(ConsoleKey key, Action<ConsoleKey> method = null, EKeyPressState pressState = EKeyPressState.None)
        {
            if (!_bindedKeys.ContainsKey(key))
                return;
            var entry = _bindedKeys[key].FirstOrDefault(b =>
                    (method == null || b.Method == method)
                    && (pressState == EKeyPressState.None || b.OnPressState == pressState)
            );
            if (entry != null)
                _bindedKeys[key].Remove(entry);
        }

        public static void Remove(Control control, Action<Control> method = null, EKeyPressState pressState = EKeyPressState.None)
        {
            if (!_bindedControls.ContainsKey(control))
                return;
            var entry = _bindedControls[control].FirstOrDefault(b =>
                    (method == null || b.Method == method)
                    && (pressState == EKeyPressState.None || b.OnPressState == pressState)
            );
            if (entry != null)
                _bindedControls[control].Remove(entry);
        }

        private static void OnTick(List<Events.TickNametagData> _)
        {
            foreach (var entry in _bindedKeys)
            {
                bool isDown = Input.IsDown((int)entry.Key);
                if (_lastKeyDownState.ContainsKey(entry.Key))
                    if (_lastKeyDownState[entry.Key] == isDown)
                        continue;
                _lastKeyDownState[entry.Key] = isDown;

                foreach (var bind in entry.Value)
                    if (isDown && bind.OnDown || !isDown && bind.OnUp)
                        bind.Method(entry.Key);
            }

            foreach (var entry in _bindedControls)
            {
                bool isDownEnabled = Pad.IsControlPressed(0, (int)entry.Key);
                bool isDownDisabled = Pad.IsDisabledControlPressed(0, (int)entry.Key);
                if (_lastControlPressedState.ContainsKey(entry.Key))
                    if (_lastControlPressedState[entry.Key] == (isDownEnabled || isDownDisabled))
                        continue;
                _lastControlPressedState[entry.Key] = (isDownEnabled || isDownDisabled);

                foreach (var bind in entry.Value)
                    if (bind.OnDown && (isDownEnabled && bind.OnEnabled || isDownDisabled && bind.OnDisabled)
                        || bind.OnUp && (!isDownEnabled && bind.OnEnabled || !isDownDisabled && bind.OnDisabled))
                        bind.Method(entry.Key);
            }
        }
    }
}