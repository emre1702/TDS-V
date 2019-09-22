using System;
using TDS_Client.Enum;
using TDS_Client.Manager.Browser;

namespace TDS_Client.Manager.Utility
{
    static class Userpanel
    {
        private static bool _open;

        public static void Open(EKey _)
        {
            if (_open)
                return;
            if (ChatManager.IsOpen)
                return;
            if (Browser.Angular.Shared.InInput)
                return;

            Browser.Angular.Main.ToggleUserpanel(true);
            _open = true;
            CursorManager.Visible = true;
        }

        public static void Close()
        {
            _open = false;
            Browser.Angular.Main.ToggleUserpanel(false);
            CursorManager.Visible = false;
        }
    }
}
