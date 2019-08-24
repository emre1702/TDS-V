using RAGE;
using RAGE.Ui;
using System;
using TDS_Client.Enum;

namespace TDS_Client.Manager.Utility
{
    internal class CursorManager
    {
        private static int _cursorOpenedCounter;

        public static bool Visible
        {
            get => Cursor.Visible;
            set
            {
                if (value)
                {
                    ++_cursorOpenedCounter;
                    Cursor.Visible = true;
                    Chat.Activate(false);
                }
                else if (--_cursorOpenedCounter <= 0)
                {
                    Cursor.Visible = false;
                    Chat.Activate(true);
                    _cursorOpenedCounter = 0;
                }
            }
        }

        public static void ManuallyToggleCursor(EKey _)
        {
            bool isVisible = Cursor.Visible;
            _cursorOpenedCounter = isVisible ? 0 : 1;
            Chat.Activate(isVisible);
            Cursor.Visible = !isVisible;
        }
    }
}