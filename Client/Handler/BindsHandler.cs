using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Models;
using static RAGE.Events;

namespace TDS_Client.Handler
{
    public class BindsHandler : ServiceBase
    {
        private readonly List<(Control, List<ControlBindDto>)> _bindedControls = new List<(Control, List<ControlBindDto>)>();
        private readonly List<(Key, List<KeyBindDto>)> _bindedKeys = new List<(Key, List<KeyBindDto>)>();
        private readonly Dictionary<Control, bool> _lastControlPressedState = new Dictionary<Control, bool>();
        private readonly Dictionary<Key, bool> _lastKeyDownState = new Dictionary<Key, bool>();

        public BindsHandler(LoggingHandler loggingHandler) : base(loggingHandler) => Tick += OnTick;

        public void Add(Key key, Action<Key> method, KeyPressState pressState = KeyPressState.Down)
        {
            var entry = _bindedKeys.FirstOrDefault(e => e.Item1 == key);
            if (entry.Item2 == null)
            {
                entry = (key, new List<KeyBindDto>());
                _bindedKeys.Add(entry);
            }
            entry.Item2.Add(new KeyBindDto(method: method, onPressState: pressState));
        }

        public void Add(Control control, Action<Control> method, KeyPressState pressState = KeyPressState.Down, bool OnEnabled = true, bool OnDisabled = false)
        {
            var entry = _bindedControls.FirstOrDefault(e => e.Item1 == control);
            if (entry.Item2 == null)
            {
                entry = (control, new List<ControlBindDto>());
                _bindedControls.Add(entry);
            }
            entry.Item2.Add(new ControlBindDto(method: method, onPressState: pressState, onEnabled: OnEnabled, onDisabled: OnDisabled));
        }

        public void Remove(Key key, Action<Key> method = null, KeyPressState pressState = KeyPressState.None)
        {
            var keyEntry = _bindedKeys.FirstOrDefault(e => e.Item1 == key);
            if (keyEntry.Item2 == null)
                return;
            var entry = keyEntry.Item2.FirstOrDefault(b =>
                    (method == null || b.Method == method)
                    && (pressState == KeyPressState.None || b.OnPressState == pressState)
            );
            if (entry != null)
                keyEntry.Item2.Remove(entry);
            if (keyEntry.Item2.Count == 0)
                _bindedKeys.Remove(keyEntry);
        }

        public void Remove(Control control, Action<Control> method = null, KeyPressState pressState = KeyPressState.None)
        {
            var controlEntry = _bindedControls.FirstOrDefault(e => e.Item1 == control);
            if (controlEntry.Item2 == null)
                return;
            var entry = controlEntry.Item2.FirstOrDefault(b =>
                    (method == null || b.Method == method)
                    && (pressState == KeyPressState.None || b.OnPressState == pressState)
            );
            if (entry != null)
                controlEntry.Item2.Remove(entry);
            if (controlEntry.Item2.Count == 0)
                _bindedControls.Remove(controlEntry);
        }

        private void OnTick(List<TickNametagData> _)
        {
            if (!RAGE.Ui.Windows.Focused)
                return;
            try
            {
                for (int i = _bindedKeys.Count - 1; i >= 0; --i)
                {
                    var keyEntry = _bindedKeys[i];
                    bool isDown = RAGE.Input.IsDown((int)keyEntry.Item1);
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
                    bool isDownEnabled = RAGE.Game.Pad.IsControlPressed((int)InputGroup.MOVE, (int)controlEntry.Item1);
                    bool isDownDisabled = RAGE.Game.Pad.IsDisabledControlPressed((int)InputGroup.MOVE, (int)controlEntry.Item1);

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
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }
    }
}
