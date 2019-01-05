﻿using RAGE;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Dto;
using TDS_Client.Enum;
using Events = RAGE.Events;

namespace TDS_Client.Manager.Utility
{
    class BindManager : Events.Script
    {
        private static readonly Dictionary<ConsoleKey, List<KeyBindDto>> bindedKeys = new Dictionary<ConsoleKey, List<KeyBindDto>>();
        private static readonly Dictionary<Control, List<ControlBindDto>> bindedControls = new Dictionary<Control, List<ControlBindDto>>();

        public BindManager()
        {
            Events.Tick += OnTick;

            Add(ConsoleKey.End, CursorManager.ManuallyToggleCursor);
        }

        public static void Add(ConsoleKey key, Action<ConsoleKey> method, EKeyPressState pressState = EKeyPressState.Down)
        {
            if (!bindedKeys.ContainsKey(key))
                bindedKeys[key] = new List<KeyBindDto>();
            bindedKeys[key].Add(new KeyBindDto() { Method = method, OnPressState = pressState });
        }

        public static void Add(Control control, Action<Control> method, EKeyPressState pressState = EKeyPressState.Down, bool OnEnabled = true, bool OnDisabled = false)
        {
            if (!bindedControls.ContainsKey(control))
                bindedControls[control] = new List<ControlBindDto>();
            bindedControls[control].Add(new ControlBindDto() { Method = method, OnPressState = pressState, OnEnabled = OnEnabled, OnDisabled = OnDisabled });
        }

        public static void Remove(ConsoleKey key, Action<ConsoleKey> method = null, EKeyPressState pressState = EKeyPressState.None)
        {
            if (!bindedKeys.ContainsKey(key))
                return;
            var entry = bindedKeys[key].FirstOrDefault(b => 
                    (method == null || b.Method == method) 
                    && (pressState == EKeyPressState.None || b.OnPressState == pressState)
            );
            if (entry != null)
                bindedKeys[key].Remove(entry);
        }

        public static void Remove(Control control, Action<Control> method = null, EKeyPressState pressState = EKeyPressState.None)
        {
            if (!bindedControls.ContainsKey(control))
                return;
            var entry = bindedControls[control].FirstOrDefault(b =>
                    (method == null || b.Method == method)
                    && (pressState == EKeyPressState.None || b.OnPressState == pressState)
            );
            if (entry != null)
                bindedControls[control].Remove(entry);
        }

        private static void OnTick(List<Events.TickNametagData> _)
        {
            foreach (var entry in bindedKeys)
            {
                bool isDown = Input.IsDown((int)entry.Key);
                bool isUp = Input.IsUp((int)entry.Key);
                foreach (var bind in entry.Value)
                {
                    if (isDown && bind.OnDown || isUp && bind.OnUp)
                        bind.Method(entry.Key);
                }
            }

            foreach (var entry in bindedControls)
            {
                bool isDownEnabled = Pad.IsControlPressed(0, (int)entry.Key);
                bool isDownDisabled = Pad.IsDisabledControlPressed(0, (int)entry.Key);
                foreach (var bind in entry.Value)
                {
                    if (bind.OnDown && (isDownEnabled && bind.OnEnabled || isDownDisabled && bind.OnDisabled)
                        || bind.OnUp && (!isDownEnabled && bind.OnEnabled || !isDownDisabled && bind.OnDisabled))
                        bind.Method(entry.Key);
                }
            }
        }

    }
}