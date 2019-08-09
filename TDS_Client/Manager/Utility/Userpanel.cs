using System;
using TDS_Client.Manager.Browser;

namespace TDS_Client.Manager.Utility
{
    static class Userpanel
    {
        private static bool _open;

        public static void Open(ConsoleKey _)
        {
            if (_open)
                return;
            Angular.ToggleUserpanel(true);
            _open = true;
            CursorManager.Visible = true;
        }

        public static void Close()
        {
            _open = false;
            Angular.ToggleUserpanel(false);
            CursorManager.Visible = false;
        }
    }
}
