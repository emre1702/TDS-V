using System;
using TDS_Client.Data.Interfaces.ModAPI.Windows;

namespace TDS_Client.RAGEAPI.Windows
{
    class WindowsAPI : IWindowsAPI
    {
        public bool Focused => RAGE.Ui.Windows.Focused;

        public bool Fullscreen => RAGE.Ui.Windows.Fullscreen;

        public void Notify(string title, string text = "", string attribute = "", int duration = 0, bool silent = false)
        {
            if (!Focused) 
                RAGE.Ui.Windows.Notify(title, text, attribute, duration, silent);
        }
    }
}
