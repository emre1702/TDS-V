using System;
using TDS_Client.Data.Enums;

namespace TDS_Client.Data.Models
{
    public class KeyBindDto
    {
        #region Public Fields

        public Action<Key> Method;
        public KeyPressState OnPressState;

        #endregion Public Fields

        #region Public Constructors

        public KeyBindDto(Action<Key> method, KeyPressState onPressState)
        {
            Method = method;
            OnPressState = onPressState;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool OnDown => OnPressState == KeyPressState.Both || OnPressState == KeyPressState.Down;
        public bool OnUp => OnPressState == KeyPressState.Both || OnPressState == KeyPressState.Up;

        #endregion Public Properties
    }
}
