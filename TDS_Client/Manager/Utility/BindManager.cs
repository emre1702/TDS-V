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
        private static readonly List<(EKey, List<KeyBindDto>)> _bindedKeys = new List<(EKey, List<KeyBindDto>)>();
        private static readonly List<(Control, List<ControlBindDto>)> _bindedControls = new List<(Control, List<ControlBindDto>)>();
        private static readonly Dictionary<EKey, bool> _lastKeyDownState = new Dictionary<EKey, bool>();
        private static readonly Dictionary<Control, bool> _lastControlPressedState = new Dictionary<Control, bool>();

        public BindManager()
        {
            Events.Tick += OnTick;

            Add(EKey.End, CursorManager.ManuallyToggleCursor);
        }

        public static void Add(EKey key, Action<EKey> method, EKeyPressState pressState = EKeyPressState.Down)
        {
            var entry = _bindedKeys.FirstOrDefault(e => e.Item1 == key);
            if (entry.Item2 == null)
            {
                entry = (key, new List<KeyBindDto>());
                _bindedKeys.Add(entry);
            }
            entry.Item2.Add(new KeyBindDto(method: method, onPressState: pressState));
        }

        public static void Add(Control control, Action<Control> method, EKeyPressState pressState = EKeyPressState.Down, bool OnEnabled = true, bool OnDisabled = false)
        {
            var entry = _bindedControls.FirstOrDefault(e => e.Item1 == control);
            if (entry.Item2 == null)
            {
                entry = (control, new List<ControlBindDto>());
                _bindedControls.Add(entry);
            }
            entry.Item2.Add(new ControlBindDto(method: method, onPressState: pressState, onEnabled: OnEnabled, onDisabled: OnDisabled));
        }

        public static void Remove(EKey key, Action<EKey> method = null, EKeyPressState pressState = EKeyPressState.None)
        {
            var keyEntry = _bindedKeys.FirstOrDefault(e => e.Item1 == key);
            if (keyEntry.Item2 == null)
                return;
            var entry = keyEntry.Item2.FirstOrDefault(b =>
                    (method == null || b.Method == method)
                    && (pressState == EKeyPressState.None || b.OnPressState == pressState)
            );
            if (entry != null)
                keyEntry.Item2.Remove(entry);
            if (keyEntry.Item2.Count == 0)
                _bindedKeys.Remove(keyEntry);
        }

        public static void Remove(Control control, Action<Control> method = null, EKeyPressState pressState = EKeyPressState.None)
        {
            var controlEntry = _bindedControls.FirstOrDefault(e => e.Item1 == control);
            if (controlEntry.Item2 == null)
                return;
            var entry = controlEntry.Item2.FirstOrDefault(b =>
                    (method == null || b.Method == method)
                    && (pressState == EKeyPressState.None || b.OnPressState == pressState)
            );
            if (entry != null)
                controlEntry.Item2.Remove(entry);
            if (controlEntry.Item2.Count == 0)
                _bindedControls.Remove(controlEntry);
        }

        private static void OnTick(List<Events.TickNametagData> _)
        {
            for (int i = _bindedKeys.Count - 1; i >= 0; --i)
            {
                var keyEntry = _bindedKeys[i];
                bool isDown = Input.IsDown((int)keyEntry.Item1);
                if (_lastKeyDownState.ContainsKey(keyEntry.Item1))
                    if (_lastKeyDownState[keyEntry.Item1] == isDown)
                        continue;
                _lastKeyDownState[keyEntry.Item1] = isDown;

                for (int j = keyEntry.Item2.Count - 1; j >= 0; --j)
                {
                    var bind = keyEntry.Item2[j];
                    if (isDown && bind.OnDown || !isDown && bind.OnUp)
                        bind.Method(keyEntry.Item1);
                }
                   
            }

            for (int i = _bindedControls.Count - 1; i >= 0; --i)
            {
                var controlEntry = _bindedControls[i];
                bool isDownEnabled = Pad.IsControlPressed(0, (int)controlEntry.Item1);
                bool isDownDisabled = Pad.IsDisabledControlPressed(0, (int)controlEntry.Item1);

                if (_lastControlPressedState.ContainsKey(controlEntry.Item1))
                    if (_lastControlPressedState[controlEntry.Item1] == (isDownEnabled || isDownDisabled))
                        continue;
                _lastControlPressedState[controlEntry.Item1] = (isDownEnabled || isDownDisabled);

                for (int j = controlEntry.Item2.Count - 1; j >= 0; --j)
                {
                    var bind = controlEntry.Item2[j];
                    if (bind.OnDown && (isDownEnabled && bind.OnEnabled || isDownDisabled && bind.OnDisabled)
                        || bind.OnUp && (!isDownEnabled && bind.OnEnabled || !isDownDisabled && bind.OnDisabled))
                        bind.Method(controlEntry.Item1);
                }
            }

        }
    }
}