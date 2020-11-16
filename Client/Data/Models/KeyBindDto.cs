using System;
using TDS.Client.Data.Enums;

namespace TDS.Client.Data.Models
{
    public class KeyBindDto
    {
        public Action<Key> Method;
        public KeyPressState OnPressState;

        public KeyBindDto(Action<Key> method, KeyPressState onPressState)
        {
            Method = method;
            OnPressState = onPressState;
        }

        public bool OnDown => OnPressState == KeyPressState.Both || OnPressState == KeyPressState.Down;
        public bool OnUp => OnPressState == KeyPressState.Both || OnPressState == KeyPressState.Up;
    }
}
