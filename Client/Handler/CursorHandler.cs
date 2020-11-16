using TDS.Client.Data.Enums;
using TDS.Client.Handler.Events;

namespace TDS.Client.Handler
{
    public class CursorHandler : ServiceBase
    {
        private readonly EventsHandler _eventsHandler;

        private readonly SettingsHandler _settingsHandler;

        private int _cursorOpenedCounter;

        public CursorHandler(LoggingHandler loggingHandler, EventsHandler eventsHandler, BindsHandler bindsHandler, SettingsHandler settingsHandler)
            : base(loggingHandler)
        {
            _eventsHandler = eventsHandler;
            _settingsHandler = settingsHandler;

            eventsHandler.ChatInputToggled += EventsHandler_ChatInputToggled;
            eventsHandler.CursorToggleRequested += b => Visible = b;

            bindsHandler.Add(Key.End, ManuallyToggleCursor);
        }

        public bool Visible
        {
            get => RAGE.Ui.Cursor.Visible;
            set
            {
                if (value)
                {
                    if (++_cursorOpenedCounter == 1)
                    {
                        RAGE.Ui.Cursor.Visible = true;
                        _eventsHandler.OnCursorToggled(true);
                    }
                }
                else if (--_cursorOpenedCounter <= 0)
                {
                    RAGE.Ui.Cursor.Visible = false;
                    _cursorOpenedCounter = 0;
                    _eventsHandler.OnCursorToggled(false);
                }
            }
        }

        public void ManuallyToggleCursor(Key _)
        {
            bool isVisible = RAGE.Ui.Cursor.Visible;
            _cursorOpenedCounter = isVisible ? 0 : 1;
            RAGE.Ui.Cursor.Visible = !isVisible;
            _eventsHandler.OnCursorToggled(!isVisible);
        }

        private void EventsHandler_ChatInputToggled(bool boolean)
        {
            if (_settingsHandler.PlayerSettings.ShowCursorOnChatOpen)
                Visible = boolean;
        }
    }
}
