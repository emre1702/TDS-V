using System;
using TDS_Client.Enum;
using TDS_Client.Manager.Browser;

namespace TDS_Client.Manager.Utility
{
    static class Userpanel
    {
        private static bool _open;

        public static void Toggle(EKey key)
        {
            if (key != EKey.NoName)
            {
                if (ChatManager.IsOpen)
                    return;
                if (Browser.Angular.Shared.InInput)
                    return;
            }
            else 
                _open = true;

            _open = !_open;
            CursorManager.Visible = _open;
            Browser.Angular.Main.ToggleUserpanel(_open);

            if (!_open)
                Settings.RevertTempSettings();
        }

        public static void Close()
        {
            Toggle(EKey.NoName);
        }
    }
}
