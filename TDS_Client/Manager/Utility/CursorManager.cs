using RAGE;
using RAGE.Ui;
using System;
using TDS_Client.Enum;

namespace TDS_Client.Manager.Utility
{
    internal class CursorManager
    {
        public static bool Visible
        {
            get => Cursor.Visible;
            set
            {
                if (value)
                {
                    if (++_cursorOpenedCounter == 1)
                    {
                        Cursor.Visible = true;
                        Chat.Activate(false);
                        CursorVisibilityChanged?.Invoke(true);
                    }
                }
                else if (--_cursorOpenedCounter <= 0)
                {
                    Cursor.Visible = false;
                    Chat.Activate(true);
                    _cursorOpenedCounter = 0;
                    CursorVisibilityChanged?.Invoke(false);
                }
            }
        }

        public delegate void CursorVisibilityChangedDelegate(bool visible);
        public static event CursorVisibilityChangedDelegate CursorVisibilityChanged;

        private static int _cursorOpenedCounter;

        public static void ManuallyToggleCursor(EKey _)
        {
            bool isVisible = Cursor.Visible;
            _cursorOpenedCounter = isVisible ? 0 : 1;
            Cursor.Visible = !isVisible;
        }
    }
}