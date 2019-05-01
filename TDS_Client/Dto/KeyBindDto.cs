using System;
using TDS_Client.Enum;

namespace TDS_Client.Dto
{
    public class KeyBindDto
    {
        public Action<ConsoleKey> Method;
        public EKeyPressState OnPressState;

        public bool OnDown => OnPressState == EKeyPressState.Both || OnPressState == EKeyPressState.Down;
        public bool OnUp => OnPressState == EKeyPressState.Both || OnPressState == EKeyPressState.Up;

        public KeyBindDto(Action<ConsoleKey> method, EKeyPressState onPressState)
        {
            Method = method;
            OnPressState = onPressState;
        }
    }
}