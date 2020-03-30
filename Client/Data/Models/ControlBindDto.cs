using System;
using TDS_Client.Data.Enums;

namespace TDS_Client.Data.Models
{
    public class ControlBindDto
    {
        public Action<Control> Method;
        public KeyPressState OnPressState;
        public bool OnEnabled;
        public bool OnDisabled;

        public bool OnDown => OnPressState == KeyPressState.Both || OnPressState == KeyPressState.Down;
        public bool OnUp => OnPressState == KeyPressState.Both || OnPressState == KeyPressState.Up;

        public ControlBindDto(Action<Control> method, KeyPressState onPressState, bool onEnabled, bool onDisabled)
        {
            Method = method;
            OnPressState = onPressState;
            OnEnabled = onEnabled;
            OnDisabled = onDisabled;
        }
    }
}
