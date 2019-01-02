using RAGE.Game;
using System;
using TDS_Client.Enum;

namespace TDS_Client.Dto
{
    public class ControlBindDto
    {
        public Action<Control> Method;
        public EKeyPressState OnPressState;
        public bool OnEnabled;
        public bool OnDisabled;

        public bool OnDown => OnPressState == EKeyPressState.Both || OnPressState == EKeyPressState.Down;
        public bool OnUp => OnPressState == EKeyPressState.Both || OnPressState == EKeyPressState.Up;
    }
}
