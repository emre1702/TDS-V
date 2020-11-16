using RAGE.Game;
using System;
using TDS.Client.Data.Enums;

namespace TDS.Client.Data.Models
{
    public class ControlBindDto
    {
        #region Public Fields

        public Action<Control> Method;
        public bool OnDisabled;
        public bool OnEnabled;
        public KeyPressState OnPressState;

        #endregion Public Fields

        #region Public Constructors

        public ControlBindDto(Action<Control> method, KeyPressState onPressState, bool onEnabled, bool onDisabled)
        {
            Method = method;
            OnPressState = onPressState;
            OnEnabled = onEnabled;
            OnDisabled = onDisabled;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool OnDown => OnPressState == KeyPressState.Both || OnPressState == KeyPressState.Down;
        public bool OnUp => OnPressState == KeyPressState.Both || OnPressState == KeyPressState.Up;

        #endregion Public Properties
    }
}
