using System;
using TDS_Client.Data.Interfaces;

namespace TDS_Client.Handler.Events
{
    public class EventsHandler
    {

        public delegate void BoolDelegate(bool boolean);
        public event BoolDelegate CursorToggled;
        public event BoolDelegate InFightStatusChanged;

        public delegate void EmptyDelegate();
        public event EmptyDelegate SettingsLoaded;

        public delegate void LanguageChangedDelegate(ILanguage lang, bool beforeLogin);
        public event LanguageChangedDelegate LanguageChanged;

        internal void OnCursorToggled(bool visible)
        {
            CursorToggled?.Invoke(visible);
        }

        internal void OnLanguageChanged(ILanguage lang, bool beforeLogin)
        {
            LanguageChanged?.Invoke(lang, beforeLogin);
        }

        internal void OnSettingsLoaded()
        {
            SettingsLoaded?.Invoke();
        }

        internal void OnInFightStatusChanged(bool value)
        {
            InFightStatusChanged?.Invoke(value);
        }

    }
}
