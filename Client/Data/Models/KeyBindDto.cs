﻿using System;
using TDS_Client.Data.Enums;

namespace TDS_Client.Data.Models
{
    public class KeyBindDto
    {
        public Action<Key> Method;
        public KeyPressState OnPressState;

        public bool OnDown => OnPressState == KeyPressState.Both || OnPressState == KeyPressState.Down;
        public bool OnUp => OnPressState == KeyPressState.Both || OnPressState == KeyPressState.Up;

        public KeyBindDto(Action<Key> method, KeyPressState onPressState)
        {
            Method = method;
            OnPressState = onPressState;
        }
    }
}