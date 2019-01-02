using RAGE;
using RAGE.Ui;
using System;
using static RAGE.Events;

namespace TDS_Client.Manager.Utility
{
    class CursorManager
    {
        private static int cursorOpenedCounter;

        public static bool Visible
        {
            get => Cursor.Visible;
            set
            {
                if (value)
                {
                    ++cursorOpenedCounter;
                    Cursor.Visible = true;
                    Chat.Activate(false);
                }
                else if (--cursorOpenedCounter <= 0)
                {
                    Cursor.Visible = false;
                    Chat.Activate(true);
                    cursorOpenedCounter = 0;
                }
            }
        } 

        public static void ManuallyToggleCursor(ConsoleKey _)
        {
            bool isVisible = Cursor.Visible;
            cursorOpenedCounter = isVisible ? 0 : 1;
            Chat.Activate(isVisible);
            Cursor.Visible = !isVisible;
        }
    }
}
